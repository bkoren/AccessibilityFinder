const token = localStorage.getItem("authToken");

document.getElementById("logoutBtn").addEventListener("click", () =>
{
    localStorage.removeItem("authToken");
    window.location.href = "/login.html";
});

document.getElementById("changePasswordBtn").addEventListener("click", () =>
{
    window.location.href = "/change-password.html";
});

document.getElementById("requestLogs").addEventListener("click", () => {
    loadLogs();
});

async function loadLogs()
{
    const logDisplayNum = document.getElementById("logDisplayNum");

    try
    {
        const CountOutput = document.getElementById("logCount");
        const getLogCount = await fetch("https://localhost:7263/log/count",
        {
            headers:
            {
                "Authorization": `Bearer ${token}`
            }
        });
        
        CountOutput.textContent = await getLogCount.json();


        if (logDisplayNum.value != "all")
        {
            const getLogNum = await fetch("https://localhost:7263/log/get/" + logDisplayNum.value,
            {
                headers:
                {
                    "Authorization": `Bearer ${token}`
                }
            });

            const logs = await getLogNum.json();
            const table = document.getElementById("logsTable");

            table.innerHTML = "";

            logs.forEach(log => {
                const row = document.createElement("tr");

                row.innerHTML = `
                    <td>${log.id}</td>
                    <td>${log.level}</td>
                    <td>${log.message}</td>
                    <td>${new Date(log.timestamp).toLocaleString()}</td>
                `;

                table.appendChild(row);
            });
        }

        else
        {
            const getLogAll = await fetch("https://localhost:7263/log/all",
            {
                headers:
                {
                    "Authorization": `Bearer ${token}`
                }
            });

            const logs = await getLogAll.json();
            const table = document.getElementById("logsTable");

            table.innerHTML = "";

            logs.forEach(log => {
                const row = document.createElement("tr");

                row.innerHTML = `
                <td>${log.id}</td>
                <td>${log.level}</td>
                <td>${log.message}</td>
                <td>${new Date(log.timestamp).toLocaleString()}</td>
            `;

                table.appendChild(row);
            });
        }

    }
    catch(error)
    {
        alert("An error occurred while loading data!");
    }
}

if (!token)
{
    alert("Access denied, login required!");
    window.location.href = "/login.html";

}
else
{
    loadLogs();
}

