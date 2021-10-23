<Query Kind="Program">
  <Reference Relative="..\src\gtdpad\bin\Debug\netcoreapp3.1\gtdpad.dll">C:\Src\gtdpad2\src\gtdpad\bin\Debug\netcoreapp3.1\gtdpad.dll</Reference>
  <Namespace>gtdpad.Domain</Namespace>
  <Namespace>Microsoft.AspNetCore.Identity</Namespace>
  <Namespace>gtdpad.Services</Namespace>
  <Namespace>gtdpad.Support</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    // Guid.NewGuid().Dump();
    
    var user = new User(
        id: new Guid("df77778f-2ef3-49af-a1a8-b1f064891ef5"), 
        email: "testuser@gtdpad.com", 
        password: "!gtdpad-test2020"
    );

    // new PasswordHasher<User>().HashPassword(user, user.Password).Dump();

    var settings = new Settings {
        ConnectionStrings = new Dictionary<string, string> {
            { "Main", "Server=localhost;Database=gtdpad;Trusted_Connection=yes" }
        }
    };
    
    var repository = new SqlServerRepository(settings);
    
    // (await repository.FindUserByEmail(user.Email)).Dump();
    
    var pageID = new Guid("796ac644-ff8e-47da-bbdc-ca8f822ce5f6");
    
    // (await repository.GetPage(pageID)).Dump();
    
    var newPageID = new Guid("576bac60-6a42-4087-9730-a317ca6012f6");
    
    var page = new Page(
        id: newPageID,
        owner: user.ID,
        title: "NEW PAGE",
        slug: "new-page"
    );
    
    // await repository.PersistPage(page);
    
    // (await repository.GetPage(newPageID)).Dump();
    
    // await repository.DeletePage(page.ID);
    
    (await repository.GetRichTextBlock(new Guid("cb60dd53-8568-499d-b640-52b7d4b5602d"))).Dump();
}