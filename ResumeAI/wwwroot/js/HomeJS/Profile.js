function toggleList() {
    const list = document.getElementById("docList");
    list.style.display = list.style.display === "none" ? "block" : "none";
}
function previewImage() {
    const input = document.getElementById('profileImageInput');
    const preview = document.getElementById('profileImagePreview');

    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function toggleList() {
    const list = document.getElementById("docList");
    list.style.display = list.style.display === "none" ? "block" : "none";
}