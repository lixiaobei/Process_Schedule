# WPF进程调度程序
使用WPF编写的进程调度

![MainWindow](https://github.com/lixiaobei/Process_Schedule/blob/master/example_photo/MainWindow.png)

主要实现功能为以下

![first_serve](https://github.com/lixiaobei/Process_Schedule/blob/master/example_photo/first_serve.png)

先来先服务算法——总是把当前处于就绪队列之首的那个进程调度到运行状态

![priority](https://github.com/lixiaobei/Process_Schedule/blob/master/example_photo/static_priority.png)

优先级调度算法——每个进程都有一个优先级与其关联,而具有最高优先级的进程会分配到 CPU。

![short_first](https://github.com/lixiaobei/Process_Schedule/blob/master/example_photo/short_first.png)

短进程优先算法——以进程的长短来计算优先级，进程越短，其优先级越高。

![simple_cycle](https://github.com/lixiaobei/Process_Schedule/blob/master/example_photo/simple_cycle.png)

简单轮转算法——每个进程被分配一时间段，称作它的时间片，即该进程允许运行的时间。
