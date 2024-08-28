using Raggle.Server.Web;
using Raggle.Server.Web.Hubs;
using Raggle.Server.Web.Options;
using Raggle.Server.Web.Services;
using Raggle.Server.Web.Storages;
using Raggle.Server.Web.Stores;

var builder = WebApplication.CreateBuilder(args);

// Configuration ����
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));

// �����ͺ��̽� ����
builder.Services.AddDatabaseServices(builder.Configuration);

// ����, ����, �ñ׳�R �����
builder.Services.AddSingleton<ConnectionStore>();
builder.Services.AddSingleton<FileStorage>();
builder.Services.AddSingleton<VectorStorage>();

// ���� ����
builder.Services.AddSingleton<ChatGenerateService>();
builder.Services.AddScoped<UserAssistantService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// �����ͺ��̽� �ʱ�ȭ
app.InitializeDatabase();

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
