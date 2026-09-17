let chosenRuleSet = null;

document.addEventListener("click", (event) => {
    const card = event.target.closest(".ruleset-card");

    // removing selection from card when clicking arount
    if (!card) {
        if (chosenRuleSet) {
            chosenRuleSet.classList.remove("chosen");
            chosenRuleSet = null;
        }
        return;
    }

    // removing selection from card already selected
    if (chosenRuleSet === card) {
        card.classList.remove("chosen");
        chosenRuleSet = null;
        return;
    }

    if (chosenRuleSet) {
        chosenRuleSet.classList.remove("chosen");
    }
    card.classList.add("chosen");
    chosenRuleSet = card;
});
