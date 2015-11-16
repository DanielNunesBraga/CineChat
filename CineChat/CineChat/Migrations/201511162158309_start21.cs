namespace CineChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class start21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "poster", c => c.String());
            DropTable("dbo.FilmesIMDBs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FilmesIMDBs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category = c.String(),
                        Year = c.Int(nullable: false),
                        Released = c.DateTime(nullable: false),
                        RunTime = c.String(),
                        Genre = c.String(),
                        Director = c.String(),
                        Writer = c.String(),
                        Actors = c.String(),
                        Plot = c.String(),
                        Language = c.String(),
                        Awards = c.String(),
                        Poster = c.String(),
                        Metascore = c.Int(nullable: false),
                        RatingIMDB = c.Single(nullable: false),
                        ImdbID = c.String(),
                        IMDBVotes = c.String(),
                        Type = c.String(),
                        Response = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Movies", "poster");
        }
    }
}
