document.getElementById("btn-submit").addEventListener("click", async function () {

    const firstName = document.getElementById("first-name");
    const lastName = document.getElementById("last-name");
    const username = document.getElementById("username");
    const email = document.getElementById("email");

    const responseInfo = document.getElementById("resonse-info");
    responseInfo.style.display = "none";
    responseInfo.className = "";

    if (firstName.value === "" && lastName.value === "" && username.value === "" && email.value === "")
        return;

    const payload = {
        firstName: firstName.value || null,
        lastName: lastName.value || null,
        email: email.value || null,
        username: username.value || null,      
    };

    try {
        const profileUpdateRequest = await fetch('https://localhost:7263/user/update', {
            method: 'PUT',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });        

        responseInfo.style.display = "block";

        if (!profileUpdateRequest.ok) {
            const error = await profileUpdateRequest.json();

            responseInfo.className = "error";

            if (profileUpdateRequest.status === 401) {
                responseInfo.innerHTML = `<p>ERROR: ${error.message}</p>`;
                return;
            }

            if (profileUpdateRequest.status === 400) {
                for (const [field, messages] of Object.entries(error.errors)) {
                    switch (field) {
                        case "FirstName":
                        case "LastName":
                        case "Email":
                        case "Username":
                            responseInfo.innerHTML = `<p>ERROR: ${messages[0]}</p>`;
                            break;
                    }
                }
                return; 
            }

            throw new Error(error.message);
        }

        responseInfo.className = "success";
        responseInfo.innerHTML = `<p>Update successful!</p>`;

    } catch (error) {
        responseInfo.className = "error";
        responseInfo.innerHTML = `<p>ERROR: ${error.message}</p>`;
    }
});
