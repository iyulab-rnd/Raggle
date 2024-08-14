using Microsoft.EntityFrameworkCore;
using Raggle.Server.Web.Database;
using Raggle.Server.Web.Hubs;
using Raggle.Server.Web.Models;
using Raggle.Server.Web.Options;
using Raggle.Server.Web.Services;
using Raggle.Server.Web.Storages;
using Raggle.Server.Web.Stores;

var builder = WebApplication.CreateBuilder(args);

// Configuration ����
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));

// �����ͺ��̽� ����
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
builder.Services.AddScoped<AppRepository<User>>();
builder.Services.AddScoped<AppRepository<Assistant>>();
builder.Services.AddScoped<AppRepository<Knowledge>>();
builder.Services.AddScoped<AppRepository<Connection>>();
builder.Services.AddScoped<AppRepository<OpenAPI>>();

// ����, ����, �ñ׳�R �����
builder.Services.AddSingleton<ConnectionStore>();
builder.Services.AddSingleton<FileStorage>();
builder.Services.AddSingleton<VectorStorage>();

// ���� ����
builder.Services.AddScoped<UserAssistantService>();
builder.Services.AddScoped<ChatGenerateService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// �����ͺ��̽� �ʱ�ȭ
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();  // ���̺� ����
}

// �̵���� ����
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

    app.UseCors(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AppHub>("/stream");

app.Run();
