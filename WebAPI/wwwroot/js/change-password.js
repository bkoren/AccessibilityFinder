
const token = localStorage.getItem("authToken");

if (!token) {
    alert("Access denied, login is required!");
    window.location.href = "/login.html";
}

document.addEventListener("DOMContentLoaded", () =>
{
    const loginForm = document.getElementById("loginForm");

    loginForm.addEventListener("submit", async (event) =>
    {
        event.preventDefault();

        const currentPassword = document.getElementById("currentPassword");
        const newPassword = document.getElementById("newPassword");

        const payload =
        {
            currentPassword: currentPassword.value,
            newPassword: newPassword.value,
        };

        try {
            const response = await fetch("https://localhost:7263/account/change-password",
                {
                    method: "POST",
                    headers:
                    {
                        "Authorization": `Bearer ${token}`,
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(payload)
                });

            if (!response.ok)
            {
                const error = await response.json();

                const currentPasswordError = document.getElementById("currentPasswordError");
                const newPasswordError = document.getElementById("newPasswordError");
                const confirmPasswordError = document.getElementById("confirmPasswordError");

                if (response.status == 401) {
                    newPasswordError.textContent = null;
                    confirmPasswordError.textContent = null;
                    currentPasswordError.textContent = error.message;

                    return;
                }

                if (response.status == 400)
                {
                    currentPasswordError.textContent = null;
                    for (const [field, messages] of Object.entries(error.errors))
                    {
       
                        switch (field) {
                            case "NewPassword":
                                confirmPasswordError.textContent = null;
                                newPasswordError.textContent = messages[0];
                                break;

                            case "ConfirmPassword":   
                                newPasswordError.textContent = null;
                                confirmPasswordError.textContent = messages[0];
                                break;
                        }
                    }
                }

                else
                    throw new Error(error.message);
            }

            window.location.href = "/login.html";
        }
        catch (error)
        {
            console.log(error);
            alert("Unexpected error occurred during process!");
        }
    });
});
