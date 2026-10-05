document.addEventListener("click", async (e) => {
 
    const formContainer = document.getElementById("activity-update");

    const editBtn = e.target.closest(".btn-for-activity");
    if (editBtn) {
        e.preventDefault();

        const row = editBtn.closest("tr");
        const activityId = row.children[0].textContent.trim();

 
        formContainer.innerHTML = `
            <h2 id="update-activity-title" style="padding-bottom:10px"></h2>
            <form id="update-activity-form" class="grid-form">
                <div class="form-group">
                    <label for="name">Name</label>
                    <input id="name" autocomplete="on" type="text">
                </div>
                <div class="form-group">
                    <label for="type">Type</label>                
                    <select class="form-type" id="type">
                        <option value="">Select Type</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="description">Description</label>
                    <textarea id="description" autocomplete="on" rows="3"></textarea>
                </div>
                <div class="form-group">
                    <label for="accessibility">Accessibilities</label>
                    <select id="accessibility" multiple="multiple"></select>
                </div>
                <div class="form-group">
                    <label for="address">Address</label>
                    <input id="address" autocomplete="on" type="text">
                </div>
                <div class="form-group">
                    <label for="contact">Contact</label>
                    <input id="contact" autocomplete="on" type="text">
                </div>
                <div class="form-group">
                    <label for="email">Email</label>
                    <input id="email" autocomplete="on" type="text">
                </div>
                <div class="full-width" style="margin-top: 10px; display: flex; gap: 10px;">
                    <button type="submit" class="btn btn-primary">Update Activity</button>
                    <span id="response-info" style="display: none"></span>
                </div>
            </form>
        `;

        async function getNames(url, elementId) {
            const response = await fetch("https://localhost:7263/" + url);
            const data = await response.json();

            const names = data.map(item => item.name);                      

            const select = document.getElementById(elementId);

            names.forEach(type => {
                const element = document.createElement("option");
                element.text = type;
                element.value = type;

                select.appendChild(element);
            });

        }

        document.getElementById("update-activity-title").innerHTML = "Update Activity id=" + activityId;

        await getNames("type/all", "type");
        await getNames("accessibility/all", "accessibility");
       
        formContainer.style.display = "block";

        document.getElementById("activity-update").scrollIntoView();

        const updateForm = document.getElementById("update-activity-form");
        updateForm.addEventListener("submit", async function (event) {
            event.preventDefault();

            const select = document.getElementById("accessibility");
            const selectedValues = Array.from(select.selectedOptions).map(opt => opt.value);

            const data = {
                Name: updateForm.name.value || null,
                Description: updateForm.description.value || null,
                Address: updateForm.address.value || null,
                Contact: updateForm.contact.value || null,
                Email: updateForm.email.value || null,
                Type: updateForm.type.value || null,
                Accessibilities: selectedValues || null
            };

            try {
                const response = await fetch(`https://localhost:7263/activity/update/${activityId}`, {
                    method: 'PUT',
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                const responseInfo = document.getElementById("response-info");

                responseInfo.style.display = "inline";
                responseInfo.style.padding = "5px";

                if (response.ok)
                {
                    responseInfo.textContent = "Activity updated successfully!";
                    responseInfo.style.color = "green";
                }
                else
                {
                    responseInfo.textContent = response.error;
                    responseInfo.style.color = "red";
                }

            }
            catch (err)
            {
                console.error(err);
            }                
        });

        return;
    }

    const deleteBtnActivity = e.target.closest(".btn-for-activity-delete");
    if (deleteBtnActivity) {
        e.preventDefault();

        const row = deleteBtnActivity.closest("tr");
        const activityId = row.children[0].textContent.trim();

        try {
            const response = await fetch(
                `https://localhost:7263/activity/delete/${activityId}`,
                {
                    method: "DELETE",
                    credentials: "include"
                }
            );

            if (response.ok)
            {
                row.remove(); 
            } 
        } catch (err)
        {
            console.error(err);

            alert("Network error");
        }
    }

    const deleteBtnReview = e.target.closest(".btn-for-review");
    if (deleteBtnReview)
    {
        e.preventDefault();

        const row = deleteBtnReview.closest("tr");
        const reviewId = row.children[0].textContent.trim();

        try
        {
            const response = await fetch(
                `https://localhost:7263/review/delete/${reviewId}`,
                {
                    method: "DELETE",
                    credentials: "include"
                }
            );

            if (response.ok)
            {
                row.remove();
            }
        }
        catch (err)
        {
            console.error(err);

            alert("Network error");
        }
    }

    const deleteBtnType = e.target.closest(".btn-for-type");
    if (deleteBtnType)
    {
        e.preventDefault();

        const row = deleteBtnType.closest("tr");
        const typeId = row.children[0].textContent.trim();

        try
        {
            const response = await fetch(
                `https://localhost:7263/type/delete/${typeId}`,
                {
                    method: "DELETE",
                    credentials: "include"
                }
            );

            if (response.ok)
            {
                row.remove();
            }
            else
            {
                const msgField = document.getElementById("jsErrorMsg");

                msgField.style = "block";
                msgField.innerHTML = await response.text();

                const clearSuccess = document.getElementById("typeSuccess");
                const clearError = document.getElementById("typeError");

                if (clearSuccess)
                    clearSuccess.innerHTML = "";

                if (clearError)
                    clearError.innerHTML = "";
            }        

            const form = document.getElementById("type-section");
            if (form) {
                form.scrollTop = form.scrollHeight;
            }
        }
        catch (err)
        {
            console.error(err);

            alert("Network error");
        }
    }

    const deleteBtnUser = e.target.closest(".btn-for-user");
    if (deleteBtnUser) {
        e.preventDefault();

        const row = deleteBtnUser.closest("tr");
        const userId = row.children[0].textContent.trim();

        try {
            const response = await fetch(
                `https://localhost:7263/user/delete/${userId}`,
                {
                    method: "DELETE",
                    credentials: "include"
                }
            );

            if (response.ok)
            {
                row.remove();
            }
        }
        catch (err) {
            console.error(err);

            alert("Network error");
        }
    }

    const deleteBtnAccessibility = e.target.closest(".btn-for-accessibility");
    if (deleteBtnAccessibility) {
        e.preventDefault();

        const row = deleteBtnAccessibility.closest("tr");
        const accessibilityId = row.children[0].textContent.trim();

        try {
            const response = await fetch(
                `https://localhost:7263/accessibility/delete/${accessibilityId}`,
                {
                    method: "DELETE",
                    credentials: "include"
                }
            );

            if (response.ok) {
                row.remove();
            }

            const form = document.getElementById("accessibility-section");
            if (form) {
                form.scrollTop = form.scrollHeight;
            }
        }
        catch (err) {
            console.error(err);

            alert("Network error");
        }
    }
});

window.addEventListener("DOMContentLoaded", () => {

    const formIds = ["type-section", "accessibility-section", "activity-section"];

    formIds.forEach(id => {
        const form = document.getElementById(id);
        if (form) {
            form.scrollTop = form.scrollHeight;
        }
    });
});


