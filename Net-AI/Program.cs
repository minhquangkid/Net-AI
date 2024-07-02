
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

//var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
////builder.Services.AddDbContext<ApplicationDbContext>(options =>
////{
////    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
////});


//// Repositories
////builder.Services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
////builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
////builder.Services.AddTransient<IUploadFileService, UploadFileService>();
////builder.Services.AddScoped<IFacebookService, FacebookService>();


//var app = builder.Build();

//// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
//app.UseSwagger();
//app.UseSwaggerUI();
////}

//app.UseCors(builder => builder
//    .AllowAnyOrigin()
//    .AllowAnyHeader()
//    .AllowAnyMethod());

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();


//app.Run();


var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion(
         "Gpt35TurboInstance",                      // Azure OpenAI Deployment Name
         "https://pbopenai.openai.azure.com/", // Azure OpenAI Endpoint
         "16617eed60a747fc9596820e561dd595");      // Azure OpenAI Key

// Alternative using OpenAI
//builder.AddOpenAIChatCompletion(
//         "gpt-3.5-turbo",                  // OpenAI Model name
//         "...your OpenAI API Key...");     // OpenAI API Key

var kernel = builder.Build();

var prompt = @"
Answer my question below :
{{$input}}
";

var summarize = kernel.CreateFunctionFromPrompt(prompt, executionSettings: new OpenAIPromptExecutionSettings { MaxTokens = 2048 }, description : "Answer question");

//{
//    "schema": 1,
//  "description": "Generate a funny joke",
//  "execution_settings": [
//    {
//        "max_tokens": 1000,
//      "temperature": 0.9,
//      "top_p": 0.0,
//      "presence_penalty": 0.0,
//      "frequency_penalty": 0.0
//    }
//  ]
//}

string text1 = @"
Who are you ?";

string text2 = @"
What's your name ?";

Console.WriteLine(await kernel.InvokeAsync(summarize, new() { ["input"] = text1 }));

Console.WriteLine(await kernel.InvokeAsync(summarize, new() { ["input"] = text2 }));

Console.ReadLine();
// Output:
//   Energy conserved, entropy increases, zero entropy at 0K.
//   Objects move in response to forces.