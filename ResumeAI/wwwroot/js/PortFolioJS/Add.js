    let serviceIndex = 1;
    let projectIndex = 1;

    $('#add-service').click(function () {
            const html = `
    <div class="service-item mb-3" data-index="${serviceIndex}">
        <input name="Services[${serviceIndex}].ServiceName" class="form-control" placeholder="Service Name" />
        <textarea name="Services[${serviceIndex}].ServiceDescription" class="form-control" placeholder="Service Description"></textarea>
        <input type="file" name="Services[${serviceIndex}].ServiceImageFile" class="form-control" />
    </div>`;
    $('#services-container').append(html);
    serviceIndex++;
        });

    $('#add-project').click(function () {
            const html = `
    <div class="project-item mb-3" data-index="${projectIndex}">
        <input name="Projects[${projectIndex}].ProjectName" class="form-control" placeholder="Project Name" />
        <textarea name="Projects[${projectIndex}].ProjectDescription" class="form-control" placeholder="Project Description"></textarea>
        <input name="Projects[${projectIndex}].ProjectAttachments" class="form-control" placeholder="Project Attachment" />
        <input name="Projects[${projectIndex}].ProjectLink" class="form-control" placeholder="Project Link" />
    </div>`;
    $('#projects-container').append(html);
    projectIndex++;
        });