let selectedNoteFile = null;
$("#file").on('change', function (e) {
    selectedNoteFile = e.target.files[0]
})

function AddEditNote() {
    let title = $("#title").val();
    let tags = $("#tags").val();
    let note = $("#note").val();


    const formData = new FormData();

    formData.append("title", title);
    formData.append("notes", note);
    formData.append("tags", tags);
    formData.append("noteId", window.NoteId);
    formData.append("file", selectedNoteFile);
    formData.append("fileName", window.NoteFileName);
    console.log("FFF")
    $.ajax({
        url: "/Notes/UpsertNotes",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (res) {
            console.log(res)
            if (res.isSuccess) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    showDenyButton: false,
                    showCancelButton: false,
                    confirmButtonText: 'Ok',
                    denyButtonText: '',
                }).then(result => {
                    window.location.href = "/Notes/Index"

                });
            }
        },
        error: function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
            })
        }
    });
}