const factsEl = document.getElementById("facts");

function reindexFacts() {
    factsEl.querySelectorAll(".fact-row").forEach((row, i) => {
        row.querySelector(".fact-object").name = `Facts[${i}].Object`;
        row.querySelector(".fact-value").name = `Facts[${i}].Value`;
    });
}

function addFactRow() {
    const row = document.createElement("div");
    row.className = "input-group mb-2 fact-row";
    row.innerHTML = `
        <input type="text" class="form-control fact-object" placeholder="Объект" list="dl-objects">
        <span class="input-group-text">=</span>
        <input type="text" class="form-control fact-value" placeholder="Значение" list="dl-values">
        <button type="button" class="btn btn-outline-danger btn-remove-fact" title="Убрать факт">&times;</button>`;
    factsEl.appendChild(row);
    reindexFacts();
    row.querySelector(".fact-object").focus();
}

document.getElementById("btn-add-fact").addEventListener("click", addFactRow);

factsEl.addEventListener("click", (event) => {
    const btn = event.target.closest(".btn-remove-fact");
    if (btn) {
        btn.closest(".fact-row").remove();
        reindexFacts();
    }
});

if (!factsEl.querySelector(".fact-row")) {
    addFactRow();
}
