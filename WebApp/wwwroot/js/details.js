const grade1 = document.getElementById("star1");
const grade2 = document.getElementById("star2");
const grade3 = document.getElementById("star3");
const grade4 = document.getElementById("star4");
const grade5 = document.getElementById("star5");

const stars = [
    grade1, grade2, grade3, grade4, grade5
];

let grade = 0;

grade1.addEventListener("click", function ()
{
    ModifyStars(1)
    grade = 1;
});

grade2.addEventListener("click", function ()
{
    ModifyStars(2)
    grade = 2;
});

grade3.addEventListener("click", function ()
{
    ModifyStars(3)
    grade = 3;
});

grade4.addEventListener("click", function ()
{
    ModifyStars(4)
    grade = 4;
});

grade5.addEventListener("click", function ()
{
    ModifyStars(5)
    grade = 5;
});

document.getElementById("btn-submit").addEventListener("click", async function ()
{
    const comment = document.getElementById("comment").value;

    var payload = 
    {
        activityId: activityId,
        comment: comment,
        grade: grade,
    }

    try
    {
        const response = await fetch('https://localhost:7263/review/add', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });

    }
    catch (error)
    {

    }
});

function ModifyStars(num)
{
    const fullStar = "fas fa-star"
    const emptyStar = "far fa-star"

    for (let i = 0; i < stars.length; i++)
    {
        stars[i].className = emptyStar;
    }

    for (let i = 0; i < num; i++)
    {
        stars[i].className = fullStar;
    }
}