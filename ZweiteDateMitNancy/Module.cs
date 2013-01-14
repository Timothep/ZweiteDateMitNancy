

namespace ZweiteDateMitNancy
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Nancy;
    using Simple.Data;
    using Simple.Data.MongoDB;
    using System.Linq;

    public class Module : NancyModule
    {
        private const string ConnectionString = @"mongodb://localhost:27017/zipDB";
        private readonly dynamic db = Database.Opener.OpenMongo(ConnectionString);

        public Module()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            Get["/zip/{id}"] = parameters =>
                {
                    CityInfo cityInfo = db.zips.FindById(parameters.id.ToString());
                    return Negotiate.WithModel(cityInfo)
                                    .WithView("SingleView");
                };

            Get["/city/{name}"] = parameters =>
                {
                    IList<CityInfo> cityInfoList = db.zips.FindAllBy(city: parameters.name.ToString()).ToList();
                    return Negotiate.WithModel(cityInfoList)
                                    .WithView("MultipleView");
                };
        }
    }
}