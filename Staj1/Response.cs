namespace Staj1
{
    public class Response
    {

        public Response()
        {
            this.Status = true;
            this.Result = "Başarılı";
        }

        public bool Status { get; set; }
        public object Value { get; set; }
        public string Result { get; set; }
    }
}
