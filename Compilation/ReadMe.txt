简易编译器
	使用Visual Studio C#实现
	基于左递归文法以及递归下降法进行整体逻辑
	文件夹说明
		utils 辅助工具类
		lex 基于输入进行词法分析获得token序列
		syntax 基于Token序列进行语法分析获得语法树
		interpreter 基于语法树进行语义分析及解释执行

可实现功能：
	主界面
		实现代码运行
		文件读取及保存
		组件随窗体可变化
		实现关键字的提示与tab键进行补全
	输入界面
		实现输入数据并返回数据

	基本数据类型
		int / float / string / bool
	变量
		赋值及使用
	全局数组 
		int a[] / float a[] / string a[] / bool a[]
		定义及赋值
	语句结束符
		$

	整数与浮点数 
		加+ /减- /乘* /除/ /取模% /乘方**
		精度最多保留5位小数
	
	字符串 
		乘法 * 重复输出与乘数相同的字符串个数，不可反转乘数与被乘数 
		相加 str + str / int / float / bool

	比较运算 
		大于> / 大于等于>= / 小于< / 小于等于<= / 等于== 
	逻辑运算
		与& / 或| / 非!
	算术运算
		加+ /减- /乘* /除/ /取模% /乘方**

	条件语句
		if 
		if else
		while
		continue
		break
	
	函数声明
		Cia
	主函数
		main
	已定义的函数
		输出函数
			print()
		输入函数
			readInt()
			readFloat()
			readString()
			readBool()
		三角函数
			sin()
			cos()
			tan()
		对数函数
			ln()
	自定义函数及调用
		可进行传参
			例 add(int a int b)
	