document.addEventListener("DOMContentLoaded", () =>
{
    const loginForm = document.getElementById("registerForm");

    loginForm.addEventListener("submit", async (event) =>
    {
        event.preventDefault();

        const firstName = document.getElementById("firstName");
        const lastName = document.getElementById("lastName");
        const username = document.getElementById("username");
        const email = document.getElementById("email");
        const password = document.getElementById("password");
        const confirm = document.getElementById("confirmPassword");

        const payload =
        {
            firstName: firstName.value,
            lastName: lastName.value,
            username: username.value,
            email: email.value,
            password: password.value,
            confirmPassword: confirm.value
        };

        try
        {
            const response = await fetch("https://localhost:7263/account/register",
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

                if (response.status == 409)
                {
                    const usernameMessage = document.getElementById("usernameError");
                    const emailMessage = document.getElementById("emailError");

                    switch (error.target)
                    {
                        case "username":
                            emailMessage.textContent = null;
                            usernameMessage.textContent = error.message;
                        break;

                        case "password":
                            usernameMessage.textContent = null;
                            emailMessage.textContent = error.message;
                        break;
                    }

                    return;
                }


                if (response.status == 400)
                {
                    const fnameOutput = document.getElementById("firstNameError");
                    const lnameOutput = document.getElementById("lastNameError");
                    const usernameOutput = document.getElementById("usernameError");
                    const emailOutput = document.getElementById("emailError");
                    const passwordOutput = document.getElementById("passwordError");
                    const confirmError = document.getElementById("confirmError");

                    fnameOutput.textContent = null;
                    lnameOutput.textContent = null;
                    usernameOutput.textContent = null;
                    emailOutput.textContent = null;
                    passwordOutput.textContent = null;
                    confirmError.textContent = null;

                    for (const [field, messages] of Object.entries(error.errors))
                    {
                        switch (field)
                        {
                            case "FirstName":
                                fnameOutput.textContent = messages[0];
                                break;

                            case "LastName":
                                lnameOutput.textContent = messages[0];
                                break;

                            case "Username":
                                usernameOutput.textContent = messages[0];
                                break;

                            case "Email":
                                emailOutput.textContent = messages[0];
                                break;

                            case "Password":
                                passwordOutput.textContent = messages[0];
                                break;

                            case "ConfirmPassword":
                                confirmError.textContent = messages[0];
                                break;                
                        }
                    }
                }

                else
                    throw new Error("Unknown error occurred!");
            }

            if (response.ok)
                window.location.href = "https://localhost:7263/login.html";

        }
        catch (error)
        {
            console.log(error);
            alert("Unexpected error during registration");
        }
    });
});
