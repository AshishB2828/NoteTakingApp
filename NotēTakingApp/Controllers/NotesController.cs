﻿using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Models;
using NoteTakingApp.Models.DTO;
using NoteTakingApp.Repository.interfaces;

namespace NoteTakingApp.Controllers
{
    public class NotesController : Controller
    {

        private readonly INoteRepository _noteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NotesController(INoteRepository noteRepository, IWebHostEnvironment webHostEnvironment)
        {
            _noteRepository = noteRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string? title, string? tag, [FromQuery]int? cpage)
        {
            var searchParams = new NoteSearchParams { SearchTitle = title, SearchTag = tag };
            var allNotes = _noteRepository.GetAllNotes(searchParams, 1, cpage??0);
            return View(allNotes);
        }

        [HttpGet]
        //[ActionName("Note")]
        public IActionResult AddEditView(string pageMode, int id)
        {
            ViewBag.PageMode = pageMode;
            ViewBag.Title = pageMode;
            var noteData = new NoteDto();

            if(pageMode  == "Edit" || pageMode == "View")
            {
               
                 if(pageMode == "Edit" || pageMode =="View")
                {
                    if (pageMode == "Edit")
                    {
                        var isAllowed = _noteRepository.IsUserAllowedToModify(id, 1);
                        if (!isAllowed) return RedirectToAction("Index");
                    }
                    noteData = _noteRepository.GetNote(id);
                }
            }
            
            return View(noteData);
        }

        [HttpPost]
        public IActionResult UpsertNotes(NoteDto noteDto, IFormFile? file )
        {
            AjaxResponse response = new AjaxResponse();

            if(!ModelState.IsValid)
            {
                return Json(response);
            }

            var fileName = "";
            if(file != null)
            {
                fileName = SaveImage(file);
                noteDto.FileName = fileName;
            }

            if(noteDto.NoteId ==0)
            {
                //Create New Note

                var note = _noteRepository.CreateNote(noteDto, 1);
                response.ResponseData = note;
                response.IsSuccess = true;

            }
            else
            {
                var isAllowed = _noteRepository.IsUserAllowedToModify(noteDto.NoteId, 1);
                if(!isAllowed)
                {
                    response.IsSuccess = false;
                    response.ResponseMessage = "Invalid Operation";
                    response.StatusCode = 403;

                }
                else
                {
                    var updatedNote = _noteRepository.UpdateNote(noteDto, 1);
                    response.ResponseData = updatedNote;
                    response.IsSuccess = true;
                }
               
            }
            
            return Json(response);

        }

        [HttpPost]
        public IActionResult DeleteNote(int noteId)
        {
            AjaxResponse response = new AjaxResponse();
            var isAllowed = _noteRepository.IsUserAllowedToModify(noteId, 1);
            if (!isAllowed)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Invalid Operation";
                response.StatusCode = 403;

            }
            else
            {
                _noteRepository.DeleteNote(noteId);
                response.IsSuccess = true;
                response.ResponseMessage = "Success";
            }
            return Json(response);
        }

        [NonAction]
        public string SaveImage(IFormFile imageFile)
        {

            string imageName = new String(Path
                    .GetFileNameWithoutExtension(imageFile.FileName)
                    .Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") +
                Path.GetExtension(imageFile.FileName);

            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                 imageFile.CopyTo(fileStream);
            }

            return imageName;
        }


    }
}
