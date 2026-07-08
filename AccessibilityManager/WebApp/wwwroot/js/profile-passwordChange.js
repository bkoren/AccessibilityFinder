document.getElementById("btn-submit").addEventListener("click", async function () {

    const currentPassword = document.getElementById("password-current");
    const newPassword = document.getElementById("password-new");

    const responseInfo = document.getElementById("resonse-info");
    responseInfo.style.display = "none";
    responseInfo.className = "";

    if (currentPassword.value === "")
        return;
    

    const payload = {
        currentPassword: currentPassword.value,
        newPassword: newPassword.value,
    };

    try {
        const passwordChangeRequest = await fetch('https://localhost:7263/account/change-password', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });        

        responseInfo.style.display = "block";

        if (!passwordChangeRequest.ok) {
            const error = await passwordChangeRequest.json();

            responseInfo.className = "error";

            if (passwordChangeRequest.status === 401) {
                responseInfo.innerHTML = `<p>ERROR: ${error.message}</p>`;
                return;
            }

            if (passwordChangeRequest.status === 400) {
                for (const [field, messages] of Object.entries(error.errors)) {
                    switch (field) {
                        case "NewPassword":
                        case "ConfirmPassword":
                            responseInfo.innerHTML = `<p>ERROR: ${messages[0]}</p>`;
                            break;
                    }
                }
                return; 
            }

            throw new Error(error.message);
        }

        responseInfo.className = "success";
        responseInfo.innerHTML = `<p>Password changed!</p>`;

    } catch (error) {
        responseInfo.className = "error";
        responseInfo.innerHTML = `<p>ERROR: ${error.message}</p>`;
    }
});
