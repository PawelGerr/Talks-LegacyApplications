class Point {
public:
	static int Count;

	int X, Y;

	Point(int x, int y) {
		X = x;
		Y = y;
		Count++;
	}

	~Point() {
		Count--;
	}

	void Add(int x, int y);
};
