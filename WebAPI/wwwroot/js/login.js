document.addEventListener("DOMContentLoaded", () =>
{
    const loginForm = document.getElementById("loginForm");

    loginForm.addEventListener("submit", async (event) =>
    {
        event.preventDefault(); 

        const username = document.getElementById("username");
        const password = document.getElementById("password");

        const usernameOutput = document.getElementById("usernameError");
        const passwordOutput = document.getElementById("passwordError");

        const payload =
        {
            username: username.value,
            password: password.value
        };

        try
        {
            const response = await fetch("https://localhost:7263/account/login",
            {
                method: "POST",
                headers:
                {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(payload)
            });


            if (!response.ok)
            {
                const error = await response.json();

                if (response.status == 401)
                {
                    switch (error.target)
                    {
                        case "username":
                            passwordOutput.textContent = null;
                            usernameOutput.textContent = error.message;
                            break;

                        case "password":
                            usernameOutput.textContent = null;
                            passwordOutput.textContent = error.message;
                            break;
                    }
                }

                return;
            }

            const data = await response.json();                  
            if (data.token)
            {
                await localStorage.setItem("authToken", data.token);

                window.location.href = "/logs.html";
            }
            else
            {
                throw new Error("No token received");
            }
        }
        catch (error)
        {
            alert("Unexpected error during login");
        }
    });
});
