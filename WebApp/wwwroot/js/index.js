let totalItems = 0;

let ItemsPerPage = 0;
let currentPage = 1;

let selectedType = 0;
let selectedAccessibilities = [];

let searchByName = "";

document.getElementById("prevPage").addEventListener("click", function ()
{
    currentPage--;
    loadPage(currentPage); 
});

document.getElementById("nextPage").addEventListener("click", function ()
{
    currentPage++;
    loadPage(currentPage);    
});

document.getElementById("apply-filters").addEventListener("click", function ()
{
    selectedAccessibilities = Array.from(
        document.querySelectorAll('input[name="accessibilities"]:checked')
    ).map(num => parseInt(num.value));
    
    selectedType = document.getElementById("type-dropdown").value;

    currentPage = 1;
    loadPage(currentPage)
});

document.getElementById("btn-search").addEventListener("click", function () {

    searchByName = document.getElementById("search-value").value;
    searchByName.trim();

    currentPage = 1;
    loadPage(currentPage)
});

async function loadPage(page)
{
    const params = new URLSearchParams();

    params.append("name", searchByName);
    params.append("page", page);
    params.append("typeId", selectedType);
    
    selectedAccessibilities.forEach(id =>
    {
        params.append("accessibilityId", id);
    });

    const requestActivities = await fetch(`https://localhost:7263/activity/search?${params.toString()}`);
    const data = await requestActivities.json();  
  
    createActivities(data);

    updatePager(data.length);    
}

function createActivities(activities)
{
    const noContent = document.getElementById("no-content");        
    const container = document.getElementById("activity-cards");

    container.innerHTML = "";
    noContent.innerHTML = "";
    noContent.style.display = "none";

    if (activities.length < 1) {
        noContent.style.display = "block";
        noContent.innerHTML = `
            <p>No activity found</p>
        `;

        return;
    }
    else if (activities.length < 4)
    {
        document.getElementById("nextPage").disabled = true;
    }

    activities.forEach(activity =>
    {
        const card = document.createElement("div");
        card.className = "card";

        card.innerHTML = `
            <div class="card-header">
                <h3 class="card-title">${activity.name}</h3>
                <span class="card-badge">${activity.type}</span>
            </div>
            <div class="card-location">
                <span class="icon">📍</span> ${(!activity.address || activity.address === "null" ? "Address not set!" : activity.address)}
            </div>
            <a class="btn-outline" href="/Activity/Activity/${activity.id}">View details</a>
        `;

        container.appendChild(card);
    });

    ItemsPerPage = activities.length;
}

async function updatePager(numOfItems)
{
    const fetchValue = await fetch("https://localhost:7263/activity/count");
    const textValue = await fetchValue.text();

    totalItems = Number(textValue);       

    document.getElementById("currentPage").textContent = currentPage;    
    document.getElementById("prevPage").disabled = (currentPage === 1);

    document.getElementById("nextPage").disabled = (numOfItems < 4 || currentPage == Math.ceil(totalItems / 4));
}

loadPage(1);

