using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPI1.Models;
using System.Web.Http.Cors;

namespace WebAPI1.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class BookController : ApiController
    {
        private string jsonFile = @"C:\Users\jyoti\source\repos\WebAPI1\WebAPI1\Data\Book.json";
        //Method to READ all the data 
        // http://localhost:18526/api/Book
        [HttpGet]
        public List<BookModel> GetAllBooks()
        {
            var json = File.ReadAllText(jsonFile);
            var bookjsondata = JObject.Parse(json);

            List<BookModel> bookDataArray = new List<BookModel>();
            var data = bookjsondata["books"];
            foreach (var item in data)
            {
                BookModel bookdata = new BookModel();
                bookdata.author = item["author"].ToString();
                bookdata.title = item["title"].ToString();
                bookdata.publisher = item["publisher"].ToString();
                bookdata.stock = item["stock"].ToString();
                bookdata.id = item["id"].ToString();
                bookDataArray.Add(bookdata);

            }
            return bookDataArray;
        }


        //Method to CREATE anew book data
        //http://localhost:18526/api/Book
        [HttpPost]
        public string PostNewBook(BookModel bookData)
        {
            try
            {
                var json = File.ReadAllText(jsonFile);
                var jsonObj = JObject.Parse(json);
                var bookArrary = jsonObj.GetValue("books") as JArray;
                String newBookJson = JsonConvert.SerializeObject(bookData);
                var newBook = JObject.Parse(newBookJson);
                bookArrary.Add(newBook);
                jsonObj["books"] = bookArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                  Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
                return "Inserted new record successfully";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();

            }
        }


        //METHOD to UPDATE boom data by book id
        // http://localhost:18526/api/Book/{id}
        [HttpPost]
        public string UpdateBook(string id, [FromBody]BookModel bookModel)
        {
            string json = File.ReadAllText(jsonFile);
            var jObject = JObject.Parse(json);
            JArray bookArray = (JArray)jObject["books"];

            foreach (var book in bookArray.Where(obj => obj["id"].Value<string>() == id))
            {
                book["author"] = bookModel.author;

                book["title"] = bookModel.title;

                book["publisher"] = bookModel.publisher;

                book["stock"] = bookModel.stock;
            }

            //jObject["experiences"] = experiencesArrary;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFile, output);
            return "hi";
        }

        //METHOD to UPDATE boom data by book id
        // http://localhost:18526/api/Book/{id}
        [HttpGet]
        public BookModel GetBookById(string id)
        {
            var json = File.ReadAllText(jsonFile);
            var bookjsondata = JObject.Parse(json);

            List<BookModel> bookDataArray = new List<BookModel>();
            BookModel bookdata = new BookModel();
            var data = bookjsondata["books"];
            foreach (var item in data.Where(obj => obj["id"].Value<string>() == id))
            {
                
                bookdata.author = item["author"].ToString();
                bookdata.title = item["title"].ToString();
                bookdata.publisher = item["publisher"].ToString();
                bookdata.stock = item["stock"].ToString();
                bookdata.id = item["id"].ToString();

            }
            return bookdata;
        }

        //Method to DELETE the book data by ID
        // http://localhost:18526/api/Book/{id}
        [HttpDelete]
        public string DeleteBookData(string id)
        {
            try
            {
                string json = File.ReadAllText(jsonFile);
                var jObject = JObject.Parse(json);
                JArray bookArray = (JArray)jObject["books"];
                var bookToDeleted = bookArray.FirstOrDefault(obj => obj["id"].Value<string>() == id);
                bookArray.Remove(bookToDeleted);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return "Selected Book has been deleted";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }



        }
    }
}