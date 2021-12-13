using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private const string filePath = "data/students.txt";
        private List<Student> studentList;
        public StudentController()
        {
            studentList = new();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    this.studentList.Add(JsonSerializer.Deserialize<Student>(line));
                }
            }
        }
        
        [HttpGet("get")]
        public List<Student> Get()
        {
            return studentList;
        }

        [HttpGet("getbyid/{studentId}/{name}")]
        public Student GetById([FromRoute] string studentId, [FromRoute] string name)
        {
            Student student = studentList.Find(x => x.ID == studentId && x.Name == name);
            if (student == null)
            {
                return null;
            }
            return student;
        }

        [HttpGet("getbyanobject/{id}/{name}/{gender}/{class}")]
        public Student GetByAnObject([FromRoute] Student studentEntity)
        {
            var student = studentList.Find(x => x.ID == studentEntity.ID && x.Name == studentEntity.Name
            && x.Gender == studentEntity.Gender && x.Class == studentEntity.Class);
            if (student == null)
            {
                return null;
            }
            return student;
        }

        [HttpPost]
        public List<Student> Post([FromBody] StudentDTO studentDto)
        {
            var student = new Student();
            var maxId = studentList.Max(x => x.ID);
            student.ID = (Convert.ToInt32(maxId) + 1).ToString();
            student.Name = studentDto.Name;
            student.Gender = studentDto.Gender;
            student.Class = studentDto.Class;
            studentList.Add(student);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var st in studentList)
                {
                    sw.WriteLine(JsonSerializer.Serialize(st));
                }
            }
            return studentList;
        }

        [HttpPut("put/{studentId}")]
        public List<Student> Put(string studentId, [FromBody] StudentDTO studentDto)
        {
            var student = studentList.Find(x => x.ID == studentId);
            if (student == null)
            {
                return null;
            }
            studentList.Remove(student);
            student.Name = studentDto.Name;
            student.Gender = studentDto.Gender;
            student.Class = studentDto.Class;
            studentList.Add(student);
            studentList = studentList.OrderBy(x => x.ID).ToList();
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var st in studentList)
                {
                    sw.WriteLine(JsonSerializer.Serialize(st));
                }
            }
            return studentList;
        }

        [HttpDelete("delete/{id}")]
        public List<Student> Delete(string id)
        {
            var student = studentList.Find(x => x.ID == id);
            if (student == null)
            {
                return null;
            }
            studentList.Remove(student);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var st in studentList)
                {
                    sw.WriteLine(JsonSerializer.Serialize(st));
                }
            }
            return studentList;
        }


        //public void Save()
        //{
        //    using (StreamWriter sw = new StreamWriter(filePath))
        //    {
        //        foreach (var st in studentList)
        //        {
        //            sw.WriteLine(JsonSerializer.Serialize(st));
        //        }
        //    }

        //}
    }
}
