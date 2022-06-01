### WCF-TCP Address,Port
> 所有服务都对应2个连续端口,分别为TCP、HTTP。
>> 1. WCF 监控服务
>>> TCP ==> net.tcp://127.0.0.1:9999/sfy/wcf/monitor
>>> HTTP ==> http://127.0.0.1:9999/sfy/wcf/monitor/imonitorcontrol
>>> 
>> 2. 开料
>>> TCP ==> net.tcp://127.0.0.1:9997/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9996/sfy/fpz/platform/cutting
>>> 
>> 3. 封边
>>> TCP ==> net.tcp://127.0.0.1:9995/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9994/sfy/fpz/platform/cutting
>>> 
>> 4. 排钻
>>> TCP ==> net.tcp://127.0.0.1:9993/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9992/sfy/fpz/platform/cutting
>>> 
>> 5. 分拣
>>> TCP ==> net.tcp://127.0.0.1:9991/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9990/sfy/fpz/platform/cutting
>>> 
>> 6. 包装
>>> TCP ==> net.tcp://127.0.0.1:9989/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9988/sfy/fpz/platform/cutting
>>> 
>> 7. 缓存架（前）
>>> TCP ==> net.tcp://127.0.0.1:9987/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9986/sfy/fpz/platform/cutting
>>> 
>> 8. 缓存架（后）
>>> TCP ==> net.tcp://127.0.0.1:9985/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9984/sfy/fpz/platform/cutting
>>> 
>> 9. 质量检测
>>> TCP ==> net.tcp://127.0.0.1:9983/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9982/sfy/fpz/platform/cutting
>> 10. 平台
>>> TCP ==> net.tcp://127.0.0.1:9981/sfy/fpz/platform/cutting
>>> HTTP ==> http://127.0.0.1:9980/sfy/fpz/platform/cutting