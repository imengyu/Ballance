---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Binding
---@field public Baselib_Memory_MaxAlignment UIntPtr 
---@field public Baselib_Memory_MinGuaranteedAlignment UIntPtr 
---@field public Baselib_NetworkAddress_IpMaxStringLength number 
---@field public Baselib_RegisteredNetwork_Buffer_Id_Invalid IntPtr 
---@field public Baselib_RegisteredNetwork_Endpoint_MaxSize number 
---@field public Baselib_Thread_InvalidId IntPtr 
---@field public Baselib_TLS_MinimumGuaranteedSlots number 
---@field public Baselib_SecondsPerMinute number 
---@field public Baselib_MillisecondsPerSecond number 
---@field public Baselib_MillisecondsPerMinute number 
---@field public Baselib_MicrosecondsPerMillisecond number 
---@field public Baselib_MicrosecondsPerSecond number 
---@field public Baselib_MicrosecondsPerMinute number 
---@field public Baselib_NanosecondsPerMicrosecond number 
---@field public Baselib_NanosecondsPerMillisecond number 
---@field public Baselib_NanosecondsPerSecond number 
---@field public Baselib_NanosecondsPerMinute number 
---@field public Baselib_Timer_MaxNumberOfNanosecondsPerTick number 
---@field public Baselib_Timer_MinNumberOfNanosecondsPerTick number 
---@field public Baselib_Memory_PageAllocation_Invalid Baselib_Memory_PageAllocation 
---@field public Baselib_RegisteredNetwork_Socket_UDP_Invalid Baselib_RegisteredNetwork_Socket_UDP 
---@field public Baselib_Socket_Handle_Invalid Baselib_Socket_Handle 
---@field public Baselib_DynamicLibrary_Handle_Invalid Baselib_DynamicLibrary_Handle 
local Binding={ }
---
---@public
---@param pathname Byte* 
---@param errorState Baselib_ErrorState* 
---@return Baselib_DynamicLibrary_Handle 
function Binding.Baselib_DynamicLibrary_Open(pathname, errorState) end
---
---@public
---@param handle Baselib_DynamicLibrary_Handle 
---@param functionName Byte* 
---@param errorState Baselib_ErrorState* 
---@return IntPtr 
function Binding.Baselib_DynamicLibrary_GetFunction(handle, functionName, errorState) end
---
---@public
---@param handle Baselib_DynamicLibrary_Handle 
---@return void 
function Binding.Baselib_DynamicLibrary_Close(handle) end
---
---@public
---@param errorState Baselib_ErrorState* 
---@param buffer Byte* 
---@param bufferLen number 
---@param verbosity number 
---@return number 
function Binding.Baselib_ErrorState_Explain(errorState, buffer, bufferLen, verbosity) end
---
---@public
---@param outPagesSizeInfo Baselib_Memory_PageSizeInfo* 
---@return void 
function Binding.Baselib_Memory_GetPageSizeInfo(outPagesSizeInfo) end
---
---@public
---@param size UIntPtr 
---@return IntPtr 
function Binding.Baselib_Memory_Allocate(size) end
---
---@public
---@param ptr IntPtr 
---@param newSize UIntPtr 
---@return IntPtr 
function Binding.Baselib_Memory_Reallocate(ptr, newSize) end
---
---@public
---@param ptr IntPtr 
---@return void 
function Binding.Baselib_Memory_Free(ptr) end
---
---@public
---@param size UIntPtr 
---@param alignment UIntPtr 
---@return IntPtr 
function Binding.Baselib_Memory_AlignedAllocate(size, alignment) end
---
---@public
---@param ptr IntPtr 
---@param newSize UIntPtr 
---@param alignment UIntPtr 
---@return IntPtr 
function Binding.Baselib_Memory_AlignedReallocate(ptr, newSize, alignment) end
---
---@public
---@param ptr IntPtr 
---@return void 
function Binding.Baselib_Memory_AlignedFree(ptr) end
---
---@public
---@param pageSize number 
---@param pageCount number 
---@param alignmentInMultipleOfPageSize number 
---@param pageState number 
---@param errorState Baselib_ErrorState* 
---@return Baselib_Memory_PageAllocation 
function Binding.Baselib_Memory_AllocatePages(pageSize, pageCount, alignmentInMultipleOfPageSize, pageState, errorState) end
---
---@public
---@param pageAllocation Baselib_Memory_PageAllocation 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Memory_ReleasePages(pageAllocation, errorState) end
---
---@public
---@param addressOfFirstPage IntPtr 
---@param pageSize number 
---@param pageCount number 
---@param pageState number 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Memory_SetPageState(addressOfFirstPage, pageSize, pageCount, pageState, errorState) end
---
---@public
---@param dstAddress Baselib_NetworkAddress* 
---@param family number 
---@param ip Byte* 
---@param port number 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_NetworkAddress_Encode(dstAddress, family, ip, port, errorState) end
---
---@public
---@param srcAddress Baselib_NetworkAddress* 
---@param family Baselib_NetworkAddress_Family* 
---@param ipAddressBuffer Byte* 
---@param ipAddressBufferLen number 
---@param port UInt16* 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_NetworkAddress_Decode(srcAddress, family, ipAddressBuffer, ipAddressBufferLen, port, errorState) end
---
---@public
---@param pageAllocation Baselib_Memory_PageAllocation 
---@param errorState Baselib_ErrorState* 
---@return Baselib_RegisteredNetwork_Buffer 
function Binding.Baselib_RegisteredNetwork_Buffer_Register(pageAllocation, errorState) end
---
---@public
---@param buffer Baselib_RegisteredNetwork_Buffer 
---@return void 
function Binding.Baselib_RegisteredNetwork_Buffer_Deregister(buffer) end
---
---@public
---@param buffer Baselib_RegisteredNetwork_Buffer 
---@param offset number 
---@param size number 
---@return Baselib_RegisteredNetwork_BufferSlice 
function Binding.Baselib_RegisteredNetwork_BufferSlice_Create(buffer, offset, size) end
---
---@public
---@return Baselib_RegisteredNetwork_BufferSlice 
function Binding.Baselib_RegisteredNetwork_BufferSlice_Empty() end
---
---@public
---@param srcAddress Baselib_NetworkAddress* 
---@param dstSlice Baselib_RegisteredNetwork_BufferSlice 
---@param errorState Baselib_ErrorState* 
---@return Baselib_RegisteredNetwork_Endpoint 
function Binding.Baselib_RegisteredNetwork_Endpoint_Create(srcAddress, dstSlice, errorState) end
---
---@public
---@return Baselib_RegisteredNetwork_Endpoint 
function Binding.Baselib_RegisteredNetwork_Endpoint_Empty() end
---
---@public
---@param endpoint Baselib_RegisteredNetwork_Endpoint 
---@param dstAddress Baselib_NetworkAddress* 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_RegisteredNetwork_Endpoint_GetNetworkAddress(endpoint, dstAddress, errorState) end
---
---@public
---@param bindAddress Baselib_NetworkAddress* 
---@param endpointReuse number 
---@param sendQueueSize number 
---@param recvQueueSize number 
---@param errorState Baselib_ErrorState* 
---@return Baselib_RegisteredNetwork_Socket_UDP 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_Create(bindAddress, endpointReuse, sendQueueSize, recvQueueSize, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param requests Baselib_RegisteredNetwork_Request* 
---@param requestsCount number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_ScheduleRecv(socket, requests, requestsCount, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param requests Baselib_RegisteredNetwork_Request* 
---@param requestsCount number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_ScheduleSend(socket, requests, requestsCount, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_ProcessRecv(socket, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_ProcessSend(socket, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param timeoutInMilliseconds number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedRecv(socket, timeoutInMilliseconds, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param timeoutInMilliseconds number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedSend(socket, timeoutInMilliseconds, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param results Baselib_RegisteredNetwork_CompletionResult* 
---@param resultsCount number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_DequeueRecv(socket, results, resultsCount, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param results Baselib_RegisteredNetwork_CompletionResult* 
---@param resultsCount number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_DequeueSend(socket, results, resultsCount, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@param dstAddress Baselib_NetworkAddress* 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_GetNetworkAddress(socket, dstAddress, errorState) end
---
---@public
---@param socket Baselib_RegisteredNetwork_Socket_UDP 
---@return void 
function Binding.Baselib_RegisteredNetwork_Socket_UDP_Close(socket) end
---
---@public
---@param family number 
---@param protocol number 
---@param errorState Baselib_ErrorState* 
---@return Baselib_Socket_Handle 
function Binding.Baselib_Socket_Create(family, protocol, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param address Baselib_NetworkAddress* 
---@param addressReuse number 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Socket_Bind(socket, address, addressReuse, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param address Baselib_NetworkAddress* 
---@param addressReuse number 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Socket_TCP_Connect(socket, address, addressReuse, errorState) end
---
---@public
---@param sockets Baselib_Socket_PollFd* 
---@param socketsCount number 
---@param timeoutInMilliseconds number 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Socket_Poll(sockets, socketsCount, timeoutInMilliseconds, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param address Baselib_NetworkAddress* 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Socket_GetAddress(socket, address, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param errorState Baselib_ErrorState* 
---@return void 
function Binding.Baselib_Socket_TCP_Listen(socket, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param errorState Baselib_ErrorState* 
---@return Baselib_Socket_Handle 
function Binding.Baselib_Socket_TCP_Accept(socket, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param messages Baselib_Socket_Message* 
---@param messagesCount number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_Socket_UDP_Send(socket, messages, messagesCount, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param data IntPtr 
---@param dataLen number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_Socket_TCP_Send(socket, data, dataLen, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param messages Baselib_Socket_Message* 
---@param messagesCount number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_Socket_UDP_Recv(socket, messages, messagesCount, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@param data IntPtr 
---@param dataLen number 
---@param errorState Baselib_ErrorState* 
---@return number 
function Binding.Baselib_Socket_TCP_Recv(socket, data, dataLen, errorState) end
---
---@public
---@param socket Baselib_Socket_Handle 
---@return void 
function Binding.Baselib_Socket_Close(socket) end
---
---@public
---@return void 
function Binding.Baselib_Thread_YieldExecution() end
---
---@public
---@return IntPtr 
function Binding.Baselib_Thread_GetCurrentThreadId() end
---
---@public
---@return UIntPtr 
function Binding.Baselib_TLS_Alloc() end
---
---@public
---@param handle UIntPtr 
---@return void 
function Binding.Baselib_TLS_Free(handle) end
---
---@public
---@param handle UIntPtr 
---@param value UIntPtr 
---@return void 
function Binding.Baselib_TLS_Set(handle, value) end
---
---@public
---@param handle UIntPtr 
---@return UIntPtr 
function Binding.Baselib_TLS_Get(handle) end
---
---@public
---@return Baselib_Timer_TickToNanosecondConversionRatio 
function Binding.Baselib_Timer_GetTicksToNanosecondsConversionRatio() end
---
---@public
---@return number 
function Binding.Baselib_Timer_GetHighPrecisionTimerTicks() end
---
---@public
---@param timeInMilliseconds number 
---@return void 
function Binding.Baselib_Timer_WaitForAtLeast(timeInMilliseconds) end
---
---@public
---@return number 
function Binding.Baselib_Timer_GetTimeSinceStartupInSeconds() end
---
Unity.Baselib.LowLevel.Binding = Binding