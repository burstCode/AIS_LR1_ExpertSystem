let chosenRuleSet = null;

document.addEventListener("click", (event) => {
    const card = event.target.closest(".ruleset-card");

    // clicks inside the modal / on the action buttons must not change selection
    if (event.target.closest(".modal, .btn")) {
        return;
    }

    // removing selection from card when clicking arount
    if (!card) {
        if (chosenRuleSet) {
            chosenRuleSet.classList.remove("chosen");
            chosenRuleSet = null;
        }
        updateButtons();
        return;
    }

    // removing selection from card already selected
    if (chosenRuleSet === card) {
        card.classList.remove("chosen");
        chosenRuleSet = null;
        updateButtons();
        return;
    }

    if (chosenRuleSet) {
        chosenRuleSet.classList.remove("chosen");
    }
    card.classList.add("chosen");
    chosenRuleSet = card;
    updateButtons();
});

function updateButtons() {
    document.getElementById("btn-edit-rule").disabled = !chosenRuleSet;
    document.getElementById("btn-remove-rule").disabled = !chosenRuleSet;
}

// rule editing modal
const modalEl = document.getElementById("rule-modal");
const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
const conditionsEl = document.getElementById("conditions");

function addConditionRow(obj = "", val = "") {
    const row = document.createElement("div");
    row.className = "mb-2 condition-row";
    row.innerHTML = `
        <div class="condition-label fst-italic fw-medium"></div>
        <div class="input-group">
            <input type="text" class="form-control cond-object" placeholder="Объект">
            <span class="input-group-text">=</span>
            <input type="text" class="form-control cond-value" placeholder="Значение">
            <button type="button" class="btn btn-outline-danger btn-remove-condition" title="Убрать условие">&times;</button>
        </div>`;
    row.querySelector(".cond-object").value = obj;
    row.querySelector(".cond-value").value = val;
    conditionsEl.appendChild(row);
    reindexConditions();
}

function reindexConditions() {
    const rows = conditionsEl.querySelectorAll(".condition-row");
    rows.forEach((row, i) => {
        row.querySelector(".condition-label").textContent = i === 0 ? "ЕСЛИ" : "И";
        const o = row.querySelector(".cond-object");
        const v = row.querySelector(".cond-value");
        o.name = `Input.Conditions[${i}].Object`;
        v.name = `Input.Conditions[${i}].Value`;
        o.required = v.required = i === 0;
        row.querySelector(".btn-remove-condition").hidden = rows.length === 1;
    });
}

function openModal(rule) {
    conditionsEl.innerHTML = "";
    document.getElementById("rule-modal-title").textContent =
        rule ? `Правило #${rule.index + 1}` : "Новое правило";
    document.getElementById("input-index").value = rule ? rule.index : "";
    document.getElementById("cons-object").value = rule ? rule.consequence.obj : "";
    document.getElementById("cons-value").value = rule ? rule.consequence.val : "";

    if (rule) {
        rule.conditions.forEach(c => addConditionRow(c.obj, c.val));
    } else {
        addConditionRow();
    }
    modal.show();
}

document.getElementById("btn-add-rule").addEventListener("click", () => openModal(null));

document.getElementById("btn-edit-rule").addEventListener("click", () => {
    if (chosenRuleSet) {
        openModal(JSON.parse(chosenRuleSet.dataset.rule));
    }
});

document.getElementById("btn-add-condition").addEventListener("click", () => addConditionRow());

conditionsEl.addEventListener("click", (event) => {
    const btn = event.target.closest(".btn-remove-condition");
    if (btn) {
        btn.closest(".condition-row").remove();
        reindexConditions();
    }
});

document.getElementById("btn-remove-rule").addEventListener("click", () => {
    if (!chosenRuleSet) return;
    const num = Number(chosenRuleSet.dataset.index) + 1;
    if (confirm(`Удалить правило #${num}?`)) {
        document.getElementById("delete-index").value = chosenRuleSet.dataset.index;
        document.getElementById("form-delete").submit();
    }
});
