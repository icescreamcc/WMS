
**本项目框架由个人编写，另外还有对应的vue3+vite的Admin前端欢迎有兴趣的朋友提供支持或改进意见（vx:13818461927）**
项目初始化：
1.修改appsettings配置文件中Sqlsugar项的数据库连接字符串（详细参数请看注释）
2.重新生成解决方案=>启动WebApi=>显示swagger界面后找到A_DbSet执行CreateDatabase接口(方法参数请看api参数说明）
3.后续开发过程中新增、修改表结构后执行AlterTable接口
4.种子数据请在Repository项目下Seed/InitData目录中创建对应表名的类，
  注意：必须实现IDbInitData接口以及对应的方法，类名必须是表名+InitData，例如初始话表SysMenus的种子数据则必须创建一个SysMenusInitData类并实现IDbInitData接口
5.由于SqlSugar没有像EF一样利用Migration命令生成对数据库操作的sql脚本，只能利用日志打印或控制台输出数据库操作的sql脚本，
  项目上线后的后续版本开发过程中需要在日志或控制台中手动筛选、复制sql脚本保存到项目DB目录中

框架功能: 
1.框架使用.NET7.0构建的webapi，预留了多租户的多数据库的处理，但目前还未实现

2.鉴权：未使用.NETCORE自带的JWT，而使用自定义的Token鉴权方式，原理是差不多的，系统内部鉴权配合AuthorizeTokenFilter过滤器，对外接口鉴权使用AuthorizeExternalFilter过滤器，具体参考AuthController控制的Action方法

3.日志：使用框架自带Nlog，主要用于记录全局异常信息，详细可以查看自定义异常过滤器OnExceptionAsync方法，业务操作日志利用ActionFilter收集并持久化到在数据库中，控制器方法需要指定BusinessLogAttribute特性，可在appsettings配置中启用或不启用

4.ORM：使用SqlSugar CodeFirst模式，理论上支持mysql、sqlserver、oracle等主流数据库，但实际使用中难保不会使用sql脚本 

5.缓存：支持Redis和MemoryCache,修改appsettings中的配置可切换,External目录对Redis和MemoryCache进行了封装,LogicBase项目中对其进一步封装便于业务逻辑层使用,具体参考CacheService目录下代码

6.依赖注入：采用.NETCORE自带的DI框架，目前不支持属性注入，后期可以考虑更换，框架功能性的注入写在Startup中，Logic业务逻辑注入统一写在Logic目录下LogicDependencyInjection项目中

7.AutoMapper：在LogicBase中引入，所有业务逻辑项目的Profile配置注入请写在LogicDependencyInjection中

8.文件存储：支持阿里OSS或本地存储模式，可在appsettings中配置选择哪种方式，如需其他云存储方式可参考LogicCommon中FileStorage目录下的代码结构

9.请求参数加密:对于前端GET请求的参数一律进行了BASE64编码,在后端过滤器ActionFilter中进行解码,如需更高要求的加密可按现有的方式在前端interceptors.request拦截器中处理加密,ActionFilter中处理解密

10.数据响应统一格式:ActionFilter过滤器中将所有状态为200的请求封装为ResponseResult对象响应给前端,ExceptionFilter将所有的异常的请求结果封装为ResponseResult对象响应给前端,响应状态码为500
   AuthorizeTokenFilter将所有鉴权未通过或没有授权的请求封装为ResponseResult对象响应给前端,响应状态码对应为具体的请求结果401或403

11.后台作业:Logic目录下ScheduleJob项目引入并封装了Quartz.NET,具体用法参考Demo目录下代码

12.消息队列:External目录下MQ项目引入并封装了RabbitMQ,在LogicBase项目中进一步封装便于业务逻辑层的使用,详细参考MQService目录下代码

13.OpenAI:集成了OpenAI接口,可以处理文本对话、图片识别及生成、语音处理（详细见External目录下OpenAI项目，以及官方文档 https://platform.openai.com/docs/api-reference/audio/create）

14.代码结构：
  1.DbRepository：数据访问层，引入了SqlSugar作为ORM框架处理数据库交互
  
  2.External：第三方提供的功能类库调用和封装的一些常用辅助类
  
  3.Logic：业务处理层
	LogicBase：所有的Logic下业务逻辑层都需要继承里面DbOperationHandler或该类的子类，DbOperationHandler中包含了Repository的实例，其子类按照业务功能而划分,具体需要继承哪个Handler,可查看具体类的注释
  
  4.Models：所有的DTO
  
  5.SysTools：提供给当前框架使用的工具小程序,主要用于加解密转换(appsettings配置中的敏感信息都是通过该工具的DES以及默认秘钥进行加解密)  
  
15.代码编写规范:
   1.要求所有控制器接口都要有注释，复杂的业务逻辑要有适当的注释
   
   2.公共方法和属性使用大驼峰命名法（例：GetUsers），私有方法、属性、字段等使用下划线+小驼峰命名法（例：_logHelper）、方法参数使用小驼峰命名法（例：userId、userName）
   
   3.async+task的异步方法一般用于处理耗时的业务,一般来说不需要访问数据库或文件IO操作等的简单业务不要使用async+task的方式定义方法,以降低性能消耗 
   
   4.数据访问层DbModels中定义数据表对应实体类，主键不要使用Id这种简单的命名方式，请使用业务描述+Id的方式定义（例：ProvinceId、LogId）
   
   5.为了使项目功能更加松散及更方便多人协作编写项目，请在Logic目录下创建各自编写业务模块，Models中对应业务模块的DTO请用对应名称的文件夹区分

   6.Action方法签名遵循以下规范:对于添加数据方法签名用Add开头,更新数据方法签名用Update开头,删除数据方法用Del开头,查询数据用Get开头,审批类的方法用Approval开头,例如:AddUser,UpdateUser,DelUser,GetUser,ApprovalSalesOrder
     拦截器会根据方法签名特点做统一消息反馈处理