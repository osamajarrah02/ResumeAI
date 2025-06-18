    let currentStep = 0;
    let certIndex = 0, eduIndex = 0, expIndex = 0, skillIndex = 0, langIndex = 0, linkIndex = 0;

    const steps = document.querySelectorAll('.step');
    const submitBtn = document.getElementById('submitBtn');

    function showStep(index) {
        steps.forEach((step, i) => {
            step.classList.toggle('d-none', i !== index);
        });
    submitBtn.classList.toggle('d-none', index !== steps.length - 1);
        }

    function nextStep() {
            if (currentStep < steps.length - 1) {
        currentStep++;
    showStep(currentStep);
            }
        }

    function prevStep() {
            if (currentStep > 0) {
        currentStep--;
    showStep(currentStep);
            }
        }
    function addEducation() {
            const container = document.getElementById('educationsContainer');
    container.insertAdjacentHTML('beforeend', `
    <div class="mb-3">
        <input class="form-control mb-2" name="Educations[${eduIndex}].InstitutionName" placeholder="Institution Name" />
        <input class="form-control mb-2" name="Educations[${eduIndex}].Degree" placeholder="Degree" />
        <input class="form-control mb-2" name="Educations[${eduIndex}].FieldOfStudy" placeholder="Field of Study" />
        <input class="form-control mb-2" name="Educations[${eduIndex}].StartDate" type="month" placeholder="Start Date" />
        <input class="form-control mb-2" name="Educations[${eduIndex}].EndDate" type="month" placeholder="End Date" />
        <input class="form-control mb-2" name="Educations[${eduIndex}].GPA" placeholder="GPA" />
        <textarea class="form-control mb-2" name="Educations[${eduIndex}].Description" placeholder="Description"></textarea>
    </div>
    `);
    eduIndex++;
        }
    function addExperience() {
            const container = document.getElementById('experiencesContainer');
    container.insertAdjacentHTML('beforeend', `
    <div class="mb-3">
        <input class="form-control mb-2" name="Experiences[${expIndex}].JobTitle" placeholder="Job Title" />
        <input class="form-control mb-2" name="Experiences[${expIndex}].CompanyName" placeholder="Company Name" />
        <input class="form-control mb-2" name="Experiences[${expIndex}].CompanyLocation" placeholder="Location" />
        <input class="form-control mb-2" name="Experiences[${expIndex}].StartDate" type="month" placeholder="Start Date" />
        <input class="form-control mb-2" name="Experiences[${expIndex}].EndDate" type="month" placeholder="End Date" />
        <input class="form-control mb-2" name="Experiences[${expIndex}].EmploymentType" placeholder="Employment Type" />
        <textarea class="form-control mb-2" name="Experiences[${expIndex}].Description" placeholder="Description"></textarea>
    </div>
    `);
    expIndex++;
        }
    function addSkill() {
            const container = document.getElementById('skillsContainer');
    container.insertAdjacentHTML('beforeend', `
    <div class="mb-3">
        <input class="form-control mb-2" name="Skills[${skillIndex}].SkillName" placeholder="Skill Name" />
        <input class="form-control mb-2" name="Skills[${skillIndex}].SkillCategory" placeholder="Skill Category" />
        <textarea class="form-control mb-2" name="Skills[${skillIndex}].SkillDescription" placeholder="Description"></textarea>
    </div>
    `);
    skillIndex++;
        }
    function addLanguage() {
            const container = document.getElementById('languagesContainer');
    container.insertAdjacentHTML('beforeend', `
    <div class="mb-3">
        <input class="form-control mb-2" name="Languages[${langIndex}].LanguageName" placeholder="Language Name" />
        <input class="form-control mb-2" name="Languages[${langIndex}].ProficiencyLevel" placeholder="Proficiency Level" />
    </div>
    `);
    langIndex++;
        }
    function addCertificate() {
            const container = document.getElementById('certificatesContainer');
    container.insertAdjacentHTML('beforeend', `
    <div class="mb-3">
        <input class="form-control mb-2" name="Certificates[${certIndex}].CourseName" placeholder="Course Name" />
        <input class="form-control mb-2" name="Certificates[${certIndex}].InstitutionName" placeholder="Institution Name" />
        <input class="form-control mb-2" name="Certificates[${certIndex}].GPA" placeholder="GPA" />
        <input class="form-control mb-2" name="Certificates[${certIndex}].CertificateType" placeholder="Certificate Type" />
        <input class="form-control mb-2" name="Certificates[${certIndex}].StartDate" type="month" />
        <input class="form-control mb-2" name="Certificates[${certIndex}].EndDate" type="month" />
    </div>
    `);
    certIndex++;
        }
    function addLink() {
            const container = document.getElementById('linksContainer');
    container.insertAdjacentHTML('beforeend', `
    <div class="mb-3">
        <input class="form-control mb-2" name="Links[${linkIndex}].LinkName" placeholder="Link Title" />
        <input class="form-control mb-2" name="Links[${linkIndex}].LinkUrl" placeholder="URL" />
    </div>
    `);
    linkIndex++;
        }
    showStep(currentStep);
    addEducation();
    addExperience();
    addSkill();
    addLanguage();
    addCertificate();
    addLink();