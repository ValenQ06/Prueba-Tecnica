[Fact]
public async Task ObtenerMunicipios_DeberiaRetornarLista()
{
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("MunicipioTestDB")
        .Options;

    var context = new ApplicationDbContext(options);

    context.Municipios.Add(new Municipio { Nombre = "Cali" });
    context.SaveChanges();

    var memoryCache = new MemoryCache(new MemoryCacheOptions());

    var service = new MunicipioService(context, memoryCache);

    var result = await service.ObtenerMunicipios();

    Assert.NotEmpty(result);
}