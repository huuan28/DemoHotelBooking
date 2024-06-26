﻿using DemoHotelBooking.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoHotelBooking.Controllers
{
    public class RoomController : Controller
    {
        public readonly AppDbContext _context;
        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Rooms.ToList();
            return View(list);
        }
        public IActionResult Details(int id)
        {
            var room = _context.Rooms.Find(id);
            if(room == null)
                return NotFound();
            return View(room);
        }

    }
}
