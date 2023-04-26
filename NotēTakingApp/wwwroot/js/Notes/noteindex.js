function ClearNoteSearch() {
    $("#tag").val('');
    $("#title").val('');
    var searchForm = $("#form-note-search");
    searchForm.submit();
}

function DeleteNote(noteId) {

    Swal.fire({
        title: '',
        icon: 'info',
        html:
            'Are you sure you want to delete this record?',
        showCloseButton: true,
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: 'red',
        cancelButtonColor: 'blue',
        confirmButtonText:
            '<i class="fa fa-trash"></i> Delete!',
        //confirmButtonAriaLabel: 'Thumbs up, great!',
        cancelButtonText:
            '<i class="fa fa-ban"></i> Cancel',
        //cancelButtonAriaLabel: 'Thumbs down',

    }).then(result => {
        if (result.isConfirmed) {
            DeleteNoteAjax(noteId)
        }
    })
}


function DeleteNoteAjax(noteId) {

    $.ajax({
        url: "/Notes/DeleteNote",
        type: "POST",
        data: { noteId },
        success: function (res) {
            console.log(res);
            if (res.isSuccess) {
                window.location.reload();
            }
        },
        error: function (err) {
            console.log(err)
            Swal.fire({
                title: '',
                icon: 'error',
                html: 'Something went wrong',
                focusConfirm: false,
                confirmButtonText: 'Ok'

            })
        }
    });

}