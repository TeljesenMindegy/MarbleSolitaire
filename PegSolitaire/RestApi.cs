using RestSharp;
using System;
using System.Collections.Generic;

namespace PegSolitaire
{
    class RestApi
    {
        private string URL = "http://localhost/ProgKorny";
        private string ROUTE = "index.php";

        public void SaveData(Scoring scoring)
        {
            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(scoring);
            IRestResponse response = client.Execute(request);
        }

        public List<Scoreboard> LoadData()
        {
            var client = new RestClient(URL);
            var request = new RestRequest(ROUTE, Method.GET);
            IRestResponse<List<Scoreboard>> response = client.Execute<List<Scoreboard>>(request);

            for (int i = 0; i < response.Data.Count; i++)
            {
                response.Data[i].Place = i + 1;
                TimeSpan time = TimeSpan.FromSeconds(double.Parse(response.Data[i].GameTime.ToString()));
                response.Data[i].TimeText = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
                
            }
            return response.Data;
        }
    }
}
