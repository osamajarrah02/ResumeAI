function buildStepsBreadcrumb(wizard, element, steps) {
    const $steps = document.getElementById(element);
    $steps.innerHTML = '';
    for (let label in steps) {
        if (steps.hasOwnProperty(label)) {
            const $li = document.createElement('li');
            const $a = document.createElement('a');
            $li.classList.add('nav-item');
            $a.classList.add('nav-link');
            if (steps[label].active) {
                $a.classList.add('active');
            }
            $a.setAttribute('href', '#');
            $a.innerText = label;
            $a.addEventListener('click', e => {
                e.preventDefault();
                wizard.revealStep(label);
            });
            $li.appendChild($a);
            $steps.appendChild($li);
        }
    }
}

function onStepChange(wizard, selector) {
    const steps = wizard.getBreadcrumb();
    buildStepsBreadcrumb(wizard, selector, steps);
}

const wizard = new window.Zangdar('#wizard', {
    onStepChange: () => {
        onStepChange(wizard, 'steps-native');
    },
    onSubmit(e) {
        e.preventDefault();
        console.log(e.target.elements);
        return false;
    }
});

onStepChange(wizard, 'steps-native');

let languageIndex = 1;
let experienceIndex = 1;
let skillIndex = 1;

function addLanguage() {
    const container = document.getElementById("language-container");
    const newItem = document.createElement("div");
    newItem.className = "language-item border p-3 rounded mb-3 bg-light";
    newItem.innerHTML = `
        <div class="row">
            <div class="col-md-6">
                <input name="CoverLetterLanguages[${languageIndex}].LanguageName" class="form-control" placeholder="Language" />
            </div>
            <div class="col-md-6">
                <input name="CoverLetterLanguages[${languageIndex}].ProficiencyLevel" class="form-control" placeholder="Proficiency" />
            </div>
        </div>
        <div class="text-end mt-2">
            <button type="button" class="btn btn-sm btn-danger" onclick="this.closest('.language-item').remove()">Remove</button>
        </div>`;
    container.appendChild(newItem);
    languageIndex++;
}

function addExperience() {
    const container = document.getElementById("experience-container");
    const newItem = document.createElement("div");
    newItem.className = "experience-item border p-3 rounded mb-3 bg-light";
    newItem.innerHTML = `
        <div class="row mb-2">
            <div class="col-md-4">
                <input name="CoverLetterExperiences[${experienceIndex}].JobTitle" class="form-control" placeholder="Job Title" />
            </div>
            <div class="col-md-4">
                <input name="CoverLetterExperiences[${experienceIndex}].CompanyName" class="form-control" placeholder="Company" />
            </div>
            <div class="col-md-4">
                <input name="CoverLetterExperiences[${experienceIndex}].CompanyLocation" class="form-control" placeholder="Location" />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-md-3">
                <input name="CoverLetterExperiences[${experienceIndex}].StartDate" class="form-control" placeholder="Start Date" />
            </div>
            <div class="col-md-3">
                <input name="CoverLetterExperiences[${experienceIndex}].EndDate" class="form-control" placeholder="End Date" />
            </div>
            <div class="col-md-3">
                <input name="CoverLetterExperiences[${experienceIndex}].EmploymentType" class="form-control" placeholder="Type" />
            </div>
        </div>
        <textarea name="CoverLetterExperiences[${experienceIndex}].Description" class="form-control" rows="2" placeholder="Role Description"></textarea>
        <div class="text-end mt-2">
            <button type="button" class="btn btn-sm btn-danger" onclick="this.closest('.experience-item').remove()">Remove</button>
        </div>`;
    container.appendChild(newItem);
    experienceIndex++;
}

function addSkill() {
    const container = document.getElementById("skills-container");
    const newItem = document.createElement("div");
    newItem.className = "skill-item border p-3 rounded mb-3 bg-light";
    newItem.innerHTML = `
        <div class="row mb-2">
            <div class="col-md-6">
                <input name="CoverLetterSkills[${skillIndex}].SkillName" class="form-control" placeholder="Skill" />
            </div>
            <div class="col-md-6">
                <input name="CoverLetterSkills[${skillIndex}].SkillCategory" class="form-control" placeholder="Category" />
            </div>
        </div>
        <textarea name="CoverLetterSkills[${skillIndex}].SkillDescription" class="form-control" rows="2" placeholder="Details"></textarea>
        <div class="text-end mt-2">
            <button type="button" class="btn btn-sm btn-danger" onclick="this.closest('.skill-item').remove()">Remove</button>
        </div>`;
    container.appendChild(newItem);
    skillIndex++;
}
