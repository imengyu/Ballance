#pragma once

template <typename T>
class SimpleLinkedList
{
public:
	SimpleLinkedList() {
		begin = nullptr;
		end = nullptr;
		m_size = 0;
	}
	~SimpleLinkedList() {
		clear();
	}

	T* begin;
	T* end;

	void add(T* n) {
		if (n == begin || n == end || n->prev)
			return;//Already in list

		n->next = nullptr;
		if (!begin || !end) {
			n->prev = nullptr;
			begin = n;
			end = n;
			m_size = 1;
		}
		else {
			n->prev = end; 
			n->next = nullptr;
			end->next = n;
			end = n;
			m_size++;
		}
	}
	void remove(T* n) {
		if (n != begin && !n->prev && !n->next)
			return;//Not in list

		if (m_size > 0) {
			if (n->prev)
				n->prev->next = n->next;
			if (n->next)
				n->next->prev = n->prev;
			if (n == begin)
				begin = n->next;
			if (n == end)
				end = n->prev;
			m_size--;

			if (m_size == 0) {
				begin = nullptr;
				end = nullptr;
			}
		}
	}
	void clear() {
		begin = nullptr;
		end = nullptr;
		m_size = 0;
	}

	int getSize() {
		return m_size;
	}

private:
	int m_size;
};

