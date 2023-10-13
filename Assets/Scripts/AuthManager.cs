using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; } // 로그인 중 로그인 시도 방지

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;
    public Button signInButton_Anonymous;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;
    public static FirebaseUser User;

    public void Start()
    {
        //Debug.Log("Start");
        signInButton.interactable = false;

        // 파이어베이스 디팬던시 픽스 // 완료 시 체인 걸기
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread( task =>
        {
            // 디팬던시 칙스 후 실행 코드

            var result = task.Result;
            
            if (result != DependencyStatus.Available) // 구동 불가
            {
                Debug.LogError(result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;

                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }

            //Debug.Log("IsFirebaseReady : " + IsFirebaseReady);
            signInButton.interactable = IsFirebaseReady;
        });
    }

    void LoginTask(System.Threading.Tasks.Task<AuthResult> task)
    {
        Debug.Log($"Sign in status : {task.Status}");

        IsSignInOnProgress = false;
        signInButton.interactable = true;

        if (task.IsFaulted)
        {
            Debug.LogError(task.Exception);
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("It's canceled");
        }
        else
        {
            User = task.Result.User;
            Debug.Log(User.Email);
            SceneManager.LoadScene("Lobby");
        }
    }

    // email login
    public void SignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }
        
        // 로그인 성공 후 체인
        // firebaseAuth.SignInWithCredentialAsync 사용
        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
        {
            LoginTask(task);
        });
    }
    public void SignUp()
    {
        string email = emailField.text;
        string password = passwordField.text;

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                // Handle the error
            }
            else if (task.IsCompleted)
            {
                // User has been created successfully
                // You can add further logic here, like redirecting to another scene.
            }
        });
    }

    // Anonymous login
    public void SignIn_Anonymous()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }

        firebaseAuth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            LoginTask(task);
        });
    }
}