# NJIS.PLC.Communication
## 概述
## 用法

## 结构分析
### Core.Net
##### IReadWriteNet => 定义和设备通信接口(读写操作)



### Core.IMessage => 定义不同设备通信消息(包)协议
#### INetMessage => 定义了通信消息
所有的消息都由消息头+内容组成
##### S7Message : INetMessage => 定义西门子S7 消息协议
**消息头：由4个字节组成**

	前2个字节用于校验数据
	byte[0]=0x03
	byte[1]=0x00
	第3-4个字节用于表示后续包内容大小
	byte[2]*256+byte[3]-4 

#### Transfer => 定义了所有接收到的消息转换（如何解析接收到的数据包）
##### IByteTransform => 定义转换接口
##### ByteTransformBase => 转换基类
##### RegularByteTransform => 常规转换
##### ReverseBytesTransform => 倒序转换
##### ReverseWordTransform => 字节错位转换


##### FinsMessage : INetMessage => 欧姆龙 Fins消息协议
**消息头：由8个字节组成**	

	前4个字节用于校验数据
	byte[0]=0x46
	byte[1]=0x49
	byte[2]=0x4E
	byte[3]=0x53
	第5-8个字节用于表示后续包内容大小
	注：字节需要翻转

##### AllenBradleyMessage : INetMessage => AB 数据包协议
**消息头：由24个字节组成**	

	包不需要校验
	前2位表示表示后续包内容大小


##### EFORTMessage : INetMessage => 埃夫特机器人数据包协议
**消息头：由18个字节组成**	

	包不需要校验
	最后2个字节-8表示后续包内容大小


##### FetchWriteMessage : INetMessage => 西门子Fetch/Write消数据包协议
**消息头：由16个字节组成**	

	前2位用于校验数据
	第12位*256+第13位表示数据包大小
	byte[12] * 256 + byte[13];

##### MelsecA1EBinaryMessage : INetMessage => 三菱的A兼容1E帧数据包协议
**消息头：由2个字节组成**	



##### MelsecQnA3EAsciiMessage : INetMessage => MC协议的Qna兼容3E帧数据包协议
**消息头：由18个字节组成**	


##### MelsecQnA3EBinaryMessage : INetMessage => Qna兼容3E帧数据包协议
**消息头：由9个字节组成**	


##### ModbusTcpMessage : INetMessage => Modbus-Tcp协议数据包协议
**消息头：由8个字节组成**	

### Core.Profinet => 基于Profinet协议的不同设备通信封装
#### Siemens(西门子)
##### Siemens 基于TCP的FETCH WRITE 通信方式
##### Siemens 使用PPI协议，通信方式
##### Siemens 使用S7协议 通信方式

#### Panasonic(松下)
#### Melsec(三菱)
#### Omron(欧姆龙)
#### AllenBradley(AB)

 
### Core.Serial => 串口通信封装

