#pragma once

template <typename T>
class SimpleStack
{
public:
	SimpleStack(int max_size) {
		m_max_size = max_size;
		m_stack = new T*[max_size];
		m_size = 0;
		memset(m_stack, 0, sizeof(T*) * max_size);
	}
	~SimpleStack() {
		delete m_stack;
		m_max_size = 0;
		m_size = 0;
	}

	bool push(T* n) {
		if (m_size < m_max_size) {
			m_stack[m_size] = n;
			m_size++;
			return true;
		}
		return false;
	}
	T* pop() {
		if (m_size > 0) {
			m_size--;
			auto v = m_stack[m_size];
			m_stack[m_size] = nullptr;
			return v;
		}
		return nullptr;
	}
	void clear() { 
		memset(m_stack, 0, sizeof(T*) * m_max_size);
		m_size = 0; 
	}
	int size() { return m_size; }

private:
	int m_size;
	int m_max_size;
	T** m_stack;
};

