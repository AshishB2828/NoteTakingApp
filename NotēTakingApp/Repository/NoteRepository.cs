using NoteTakingApp.Data;
using NoteTakingApp.Models;
using NoteTakingApp.Models.DTO;
using NoteTakingApp.Repository.interfaces;
using System;

namespace NoteTakingApp.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _appContext;

        public NoteRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public NoteDto CreateNote(NoteDto note, int currentUser)
        {
            Note newNote = new Note
            {
                FileName = note.FileName,
                NoteId = note.NoteId,
                Notes = note.Notes,
                Tags = note.Tags,
                Title = note.Title
            };

            newNote.CreatedAt = DateTime.Now;
            newNote.CreatedBy = currentUser;

            _appContext.Notes.Add(newNote);
            _appContext.SaveChanges();
            note.NoteId = newNote.NoteId;
            return note;

        }

        public NoteDto DeleteNote(int noteId)
        {
            var noteInDb = _appContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
            if (noteInDb == null) return null;
            _appContext.Notes.Remove(noteInDb);
            _appContext.SaveChanges();

            var noteDto = new NoteDto
            {
                CreatedAt = noteInDb.CreatedAt,
                CreatedBy = noteInDb.CreatedBy,
                FileName = noteInDb.FileName,
                NoteId = noteInDb.NoteId,
                Notes = noteInDb.Notes,
                Tags = noteInDb.Tags,
                Title = noteInDb.Title
            };
            return noteDto;
        }

        public NoteDto GetNote(int noteId)
        {
            var noteInDb = _appContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
            if (noteInDb == null) return null;

            var noteDto = new NoteDto
            {
                CreatedAt = noteInDb.CreatedAt,
                CreatedBy = noteInDb.CreatedBy,
                FileName = noteInDb.FileName,
                NoteId = noteInDb.NoteId,
                Notes = noteInDb.Notes,
                Tags = noteInDb.Tags,
                Title = noteInDb.Title
            };
            return noteDto;
        }

        public bool IsUserAllowedToModify(int noteId, int userId)
        {
            var noteInDb = _appContext.Notes.FirstOrDefault(x => x.NoteId == noteId);
            if (noteInDb == null) return false;
            return noteInDb.CreatedBy == userId;
        }

        public NoteDto UpdateNote(NoteDto note, int currentUser)
        {
            var noteInDb = _appContext.Notes.FirstOrDefault(x => x.NoteId == note.NoteId);
            if (noteInDb == null) return null;
            noteInDb.Title = note.Title;
            noteInDb.Notes = note.Notes;
            noteInDb.Tags = note.Tags;
            noteInDb.FileName = note.FileName;
            _appContext.SaveChanges();
           var noteDto =  new NoteDto
            {
                CreatedAt = noteInDb.CreatedAt,
                CreatedBy = noteInDb.CreatedBy,
                FileName = noteInDb.FileName,
                NoteId = noteInDb.NoteId,
                Notes = noteInDb.Notes,
                Tags = noteInDb.Tags,
                Title = noteInDb.Title
            };
            return noteDto;
        }

        public  List<NoteDto> GetAllNotes(int currentUserId)
        {
            var notesInDb = _appContext.Notes.Where(x => x.CreatedBy == currentUserId)
                                .Select(note => new NoteDto
                                        {
                                            CreatedAt = note.CreatedAt,
                                            CreatedBy   = note.CreatedBy,
                                            FileName = note.FileName,
                                            NoteId = note.NoteId,
                                            Notes = note.Notes,
                                            Tags = note.Tags,
                                            Title = note.Title
                                        })
                                .OrderBy(x => x.CreatedAt)        
                                .ToList();
            return notesInDb;
        }
    }
}
