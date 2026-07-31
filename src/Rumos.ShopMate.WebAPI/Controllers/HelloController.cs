using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // GET https://localhsot:4567/api/hello 
public class HelloController : ControllerBase
{
    private readonly HelloService _service;
    private readonly SingletonExample _singleton;
    private readonly ScopedExample _scoped; // new ScopedExample() // x010
    private readonly TransientExample _transient;


    public HelloController(HelloService service, SingletonExample singleton, ScopedExample scoped, TransientExample transient)
    {
        _service = service;
        _singleton = singleton;
        _scoped = scoped;
        _transient = transient;
    }
    
    [HttpGet]
    public ActionResult<object> GetAll()
    {

        var result = _service.Get();
        
        return new
        {
            Scoped = _scoped.Id,
            Transient = _transient.Id,
            Singleton = _singleton.Id,
            Result = result
        };
    }
}

// AddScoped
// AddTransient

// AddSingleton

// var se = new SingletonExample(); // x009 => asdf-asdf-asdf
// cw(se.Guid); =>  asdf-asdf-asdf
// cw(se.Guid); =>  asdf-asdf-asdf

// var see = new SingletonExample(); // x010 => asdfe-asdfe-asdfe
// cw(se.Guid); =>  asdf 
// cw(see.Guid); =>   asdfe




public class SingletonExample {

    public Guid Id { get;  } = Guid.NewGuid();
}

public class ScopedExample
{
    public Guid Id { get;  } = Guid.NewGuid();
}

public class TransientExample
{
    public Guid Id { get;  } = Guid.NewGuid();
}

public class HelloService(SingletonExample singleton, ScopedExample scoped, TransientExample transient)
{

    public object Get()
    {
        return new
        {
            Scoped = scoped.Id,
            Transient = transient.Id,
            Singleton = singleton.Id
        };
    }
    
}