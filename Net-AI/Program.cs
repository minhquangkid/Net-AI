
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using Kernel = Microsoft.SemanticKernel.Kernel;
using System;
using System.Text;


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


// Get AI service instance used to manage the user chat
var chatGPT = kernel.GetRequiredService<IChatCompletionService>();

var systemMessage = "Đây là cuộc trò chuyện giữa một bot AI có tên là Bé Na và một khách hàng muốn đặt mua bánh của tiệm " +
    "\nBé Na là nhân viên của tiệm bánh đang tư vấn và thu thập thông tin từ khách hàng để tạo đơn hàng " +
    "\nThứ tự trao đổi với khách phải bắt buộc tuân thủ theo các bước sau. Bước 1 là xin họ tên và số điện thoại của khách hàng và nói câu xin chào như sau" +
    "\" 'Xin chào {customerName}. Rất vui được gặp bạn' với customerName là tên của khách hàng đã cung cấp." +
    "\nBước 2 là phải hỏi khách mua Bánh gì, kích cỡ như thế nào." +
    "\nBước 3 là hỏi thời gian khách hàng đến lấy bánh,và hỏi khách hàng có muốn tiệm giao bánh đến nhà của khách hàng hay không ? Nếu khách hàng đồng ý giao bánh thì phải hỏi địa điểm để giao bánh." +
    "\nBước 4 là tạo đơn bánh từ những thông tin đã thu thập được ở các bước trước đó " +
    "\nCuộc trò chuyện sử dụng hoàn toàn bằng Tiếng Việt";
             

var chat = new ChatHistory(systemMessage);

Console.OutputEncoding = Encoding.UTF8; // using Unicode

while (true)
{
    // 1. Ask the user for a message. The user enters a message.  Add the user message into the Chat History object.
    var userMessage = Console.ReadLine();
    Console.WriteLine($"User: {userMessage}");
    chat.AddUserMessage(userMessage);

    // 2. Send the chat object to AI asking to generate a response. Add the bot message into the Chat History object.
    var assistantReply = await chatGPT.GetChatMessageContentAsync(chat, new OpenAIPromptExecutionSettings());
    chat.AddAssistantMessage(assistantReply.Content);

    // 3. Show the reply as an image
    Console.WriteLine($"\nBot:");
    Console.WriteLine($"[{assistantReply}]\n");
}