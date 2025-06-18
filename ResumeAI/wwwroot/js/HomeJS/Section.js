document.querySelectorAll('.color-box').forEach(box => {
    box.addEventListener('click', () => {
        const bgColor = box.getAttribute('data-bg');
        const textColor = box.getAttribute('data-text');
        const resumeCard = document.querySelector('.resume-card');

        // Remove Bootstrap bg class to allow style override
        resumeCard.classList.remove('bg-white');
        resumeCard.style.backgroundColor = bgColor;
        resumeCard.style.setProperty('color', textColor, 'important');

        // Update nested text elements
        resumeCard.querySelectorAll('h4, h6, p, li, strong, small, span, div')
            .forEach(el => {
                el.classList.forEach(cls => {
                    if (cls.startsWith('text-')) el.classList.remove(cls);
                });
                el.style.setProperty('color', textColor, 'important');
            });
    });
});