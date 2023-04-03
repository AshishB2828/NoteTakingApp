using NoteTakingApp.Data;
using NoteTakingApp.Models;
using NoteTakingApp.Models.DTO;
using NoteTakingApp.Repository.interfaces;
using System;
using System.Linq;

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

        public NoteList GetAllNotes(NoteSearchParams searchParams, int currentUserId, int cpage)
        {
            if (cpage == 0)
            {
                cpage = 1;
            }

            var notesInDbQuery = _appContext.Notes.Where(x => x.CreatedBy == currentUserId);
                              

            if (!string.IsNullOrEmpty(searchParams.SearchTitle))
            {
                notesInDbQuery = notesInDbQuery.Where(i => i.Title.ToLower().Contains(searchParams.SearchTitle.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchParams.SearchTag))
            {
                notesInDbQuery = notesInDbQuery.Where(i => i.Tags.ToLower().Contains(searchParams.SearchTag.ToLower()));
            }
            int itemPerPage = 2;
            int totalItems =notesInDbQuery.Count();

            int totalPages = (int)Math.Ceiling(totalItems / (double)itemPerPage);


            //notesInDbQuery = notesInDbQuery.Skip(cpage - 1).Take(itemPerPage);
            int skip = (cpage - 1) * itemPerPage;
            if(cpage > 0)
            {
                notesInDbQuery = notesInDbQuery.Skip(skip).Take(itemPerPage);
            }
            notesInDbQuery = notesInDbQuery.Take(itemPerPage);

            var notesInDb = notesInDbQuery
                             .Select(note => new NoteDto
                             {
                                 CreatedAt = note.CreatedAt,
                                 CreatedBy = note.CreatedBy,
                                 FileName = note.FileName,
                                 NoteId = note.NoteId,
                                 Notes = note.Notes,
                                 Tags = note.Tags,
                                 Title = note.Title
                             })
                            .OrderBy(x => x.CreatedAt)
                            
                                                .ToList();


            var listWithPagination = new NoteList 
                            { Notes = notesInDb, CurrentPage = cpage, 
                              PageSize = itemPerPage, TotalPage = totalPages, 
                            SearchTag = searchParams.SearchTag, SearchTitle = searchParams.SearchTitle 
            };
            return listWithPagination;
        }
    }
}
