#include "main.h"
#include <iostream>
#include <WinSock2.h>
#include <Ws2tcpip.h> 
#include <string>
#include <intrin.h>
#pragma comment(lib, "shlwapi.lib")
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "netapi32.lib")

#define DEFAULT_PORT 2566//默认端口号 
#define BACKLOG 5/*最大监听数*/ 
#define RECV_BUFFER_SIZE 512//接收发送缓冲区大小 
#define SEND_BUFFER_SIZE 2048//接收发送缓冲区大小 

SOCKET sockfd, new_fd; /*socket句柄和建立连接后的句柄*/
struct sockaddr_in my_addr; /*本方地址信息结构体，下面有具体的属性赋值*/
struct sockaddr_in their_addr; /*对方地址信息*/
int port = DEFAULT_PORT;
int sin_size;
int timeout = 300;
int buf = 10240;
HANDLE hThread = nullptr;
bool running = false;

enum SendBackInfoType {
  SendBackInfoTypeInt,
  SendBackInfoTypePtr,
};

struct SendBackInfoHeader {
  int success;
  int bufferType;
  long long bufferSize;
};


std::string IPToString(sockaddr_in* addr)
{
  char sendBuf[100] = { '\0' };
  inet_ntop(AF_INET, (void*)&addr->sin_addr, sendBuf, 100);
  return std::string(sendBuf);
}

bool serverLoop()
{
  char recv_buf[RECV_BUFFER_SIZE];
  char send_buf[SEND_BUFFER_SIZE];

  printf_s("Server work start\n");

  int recvTimeout = 30 * 1000;
  int sendTimeout = 30 * 1000;

  int len = sizeof(SOCKADDR);
  new_fd = accept(sockfd, (SOCKADDR*)&their_addr, &len);
  //在这里阻塞 挂起等待

  if (new_fd == SOCKET_ERROR)
  {
    printf_s("receive failed\n");
    closesocket(sockfd);
    return false;
  }

  //设置超时时间
  setsockopt(new_fd, SOL_SOCKET, SO_RCVTIMEO, (char*)&recvTimeout, sizeof(int));
  setsockopt(new_fd, SOL_SOCKET, SO_SNDTIMEO, (char*)&sendTimeout, sizeof(int));

  printf_s("New connection: %s\n", IPToString(&their_addr).c_str());

  while (running) {

    memset(recv_buf, 0, sizeof(recv_buf));

    int recv_len = recv(new_fd, recv_buf, RECV_BUFFER_SIZE, 0);
    if (recv_len <= 0) //这里 即使链接断开 也能继续监听 服务端不关闭
    {
      closesocket(new_fd);
      printf_s("Connection close\n");

      sin_size = sizeof(struct sockaddr_in);
      new_fd = accept(sockfd, (struct sockaddr*)&their_addr, &sin_size);//在这里阻塞知道接收到消息，参数分别是socket句柄，接收到的地址信息以及大小 
      if (new_fd == SOCKET_ERROR) {
        printf_s("Server receive failed\n");
        return false;
      }
      else {

        //设置超时时间
        setsockopt(new_fd, SOL_SOCKET, SO_RCVTIMEO, (char*)&recvTimeout, sizeof(int));
        setsockopt(new_fd, SOL_SOCKET, SO_SNDTIMEO, (char*)&sendTimeout, sizeof(int));

        printf_s("New connection: %s\n", IPToString(&their_addr).c_str());
        continue;
      }
    }

    bool sendBack = false;
    size_t sendBackSize = SEND_BUFFER_SIZE;
    SendBackInfoHeader sendBackHeader = { 0 };

    if (strcmp(recv_buf, "") != 0) {
      //处理API数据
      if (strncmp(recv_buf, "Loader_GetLastError", 20) == 0) {
        int result = Loader_GetLastError();
        sendBackHeader.bufferSize = sizeof(int);
        sendBackHeader.bufferType = SendBackInfoTypeInt;
        sendBack = true;
        memcpy_s(send_buf, sizeof(send_buf), &sendBackHeader, sizeof(sendBackHeader));
        memcpy_s(send_buf + sizeof(sendBackHeader), sizeof(send_buf) - sizeof(sendBackHeader), &result, sizeof(result));
      }
      else if (strncmp(recv_buf, "Loader_SolveNmoFileRead", 24) == 0) {
        int resultCode = 0;
        void* result = Loader_SolveNmoFileRead(recv_buf + 25, &resultCode);
        void* ptr = send_buf + sizeof(sendBackHeader);
        sendBackHeader.bufferSize = sizeof(int);
        sendBackHeader.bufferType = SendBackInfoTypeInt;
        sendBack = true;
        memcpy_s(send_buf, sizeof(send_buf), &sendBackHeader, sizeof(sendBackHeader));
        memcpy_s(ptr, sizeof(send_buf), &result, sizeof(result)); 
        ptr = (void*)((intptr_t)ptr + sizeof(result));
        memcpy_s(ptr, sizeof(send_buf), &resultCode, sizeof(resultCode));
      }
      
      
    }
  SEND:
    if (sendBack) {
      if (send(new_fd, send_buf, sendBackSize, 0) == -1)
        printf_s("%s -> Send failed! \n", recv_buf);
    }
  }

  printf_s("Server Quit\n");

  closesocket(new_fd);
  closesocket(sockfd);
  return true;
}
bool startServer() {
  WSADATA wsaData;
  if (WSAStartup(MAKEWORD(2, 0), &wsaData) != 0)
  {
    printf_s("WSAStartup failed: %d", WSAGetLastError());
    return false;
  }

  sockfd = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);//建立socket 
  if (sockfd == SOCKET_ERROR) {
    printf_s("socket failed : % d", errno);
    return false;
  }

  my_addr.sin_family = AF_INET;/*该属性表示接收本机或其他机器传输*/
  my_addr.sin_port = htons(port);/*端口号*/
  my_addr.sin_addr.s_addr = htonl(INADDR_ANY);/*IP，括号内容表示本机IP*/
  ZeroMemory(&my_addr.sin_zero, sizeof(my_addr.sin_zero));/*将其他属性置0*/

  if (bind(sockfd, (struct sockaddr*)&my_addr, sizeof(struct sockaddr)) < 0) {//绑定地址结构体和socket
    printf_s("bind error");
    return false;
  }

  if (listen(sockfd, BACKLOG) == SOCKET_ERROR)//开启监听 ，第二个参数是最大监听数 
  {
    printf_s("listen error");
    return false;
  }

  int recvTimeout = 10 * 1000;
  int sendTimeout = 10 * 1000;

  setsockopt(sockfd, SOL_SOCKET, SO_RCVTIMEO, (char*)&recvTimeout, sizeof(int));
  setsockopt(sockfd, SOL_SOCKET, SO_SNDTIMEO, (char*)&sendTimeout, sizeof(int));


  return true;
}
bool stopServer()
{
  if (running) {
    running = false;
  }
  if (sockfd > 0) {
    shutdown(sockfd, SD_BOTH);
    closesocket(sockfd);
    sockfd = 0;
  }
  WSACleanup();
  return false;
}

//主函数

int main(int argc, char const* argv[])
{
  char recv_buf[15];
  sprintf_s(recv_buf, "Test string");


  return 0;
  //路径
  char fullPath[512];
  GetModuleFileName(0, fullPath, 512);
  for (int i = strlen(fullPath) - 1; i >= 0; i--)
    if (fullPath[i] == '\\') {
      fullPath[i + 1] = '\0';
      printf_s("Full path is : %d\n", port);
      break;
    }

  //参数
  for (int i = 1; i < argc; i++)
  {
    //Port 参数
    if (strcmp(argv[i - 1], "-port") == 0 && strcmp(argv[i], "") != 0) {
      port = atoi(argv[i]);
      printf_s("Args port: %d\n", port);
    }
  }

  //初始化库
  if (Loader_Init(GetConsoleWindow(), fullPath))
    return -1;

  //开启服务器
  if (!startServer())
    return -1;

  if (!serverLoop())
    return -1;


  //停止服务
  stopServer();

  //卸载
  return Loader_Destroy();
}