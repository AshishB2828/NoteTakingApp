using NoteTakingApp.Models.DTO;

namespace NoteTakingApp.Repository.interfaces
{
    public interface INoteRepository
    {

        NoteDto CreateNote(NoteDto note, int currentUser);
        NoteDto UpdateNote(NoteDto note, int currentUser);
        NoteDto DeleteNote(int id);
        NoteDto GetNote(int id);
        NoteList GetAllNotes(NoteSearchParams searchParams, int currentUserId, int cpage);
        bool IsUserAllowedToModify(int noteId, int userId);

    }
}
