#ifndef RANDOMGENERATOR_H

#define RANDOMGENERATOR_H


/////////////////////////////////////////////////
// Random Generators
// created  : AGoTH
// date		: 01/12/01
/////////////////////////////////////////////////
#include <cstdlib>

// 
template <class R>
float
GaussianDistribution(const R& iRG, float iMean, float iDeviation)
{
//  Generic form of the Box-Muller transformation
//	float x1 = iRG();
//	float x2 = iRG();
//	float y1 = XSqrt( -2.0f * logf(x1) ) * cos( 2 * 3.14f * x2 );
//  float y2 = XSqrt( -2.0f * logf(x1) ) * sin( 2 * 3.14f * x2 );
//  We will use the Polar form for efficiency

	static int useLast = 0;
	static float y2;
	float y1;

	if (useLast) {
		y1		= y2;
		useLast = 0;
	} else {
		float x1, x2, w;
		do {
			x1 = 2.0f * iRG() - 1.0f;
			x2 = 2.0f * iRG() - 1.0f;
			w = x1 * x1 + x2 * x2;
		} while ( w >= 1.0f );
		
		// I need to /4 otherwise it pass the bounds !
		w		= 0.25f * XSqrt( (-2.0f * logf(w) )/w);
		y1		= x1 * w;
		y2		= x2 * w;
		useLast	= 1;
	}

	return (iMean + y1 * iDeviation);
}

// 
class StandardRandomGenerator
{
public:
	StandardRandomGenerator()
	{
	}

	// Initialize the random generator
	// for a given seed, the sequence
	// of generated number will be the same
	void	Init(unsigned int iSeed) const
	{
		// sets a random starting point
		srand(iSeed);
	}

	// Returns an uniformely distribued float 
	// value : [0,1)
	float	operator ()() const
	{
		return (INVRANDMAX*rand());
	}

	// returns a random number distribued 
	// "gaussianly" with a mean and a deviation
	// value : [iMean-iDeviation,iMean+iDeviation)
	float	Gaussian(float iMean, float iDeviation) const
	{
		return GaussianDistribution(*this,iMean,iDeviation);
	}

private:
	static const float INVRANDMAX;
};

const float StandardRandomGenerator::INVRANDMAX = 1.0f/RAND_MAX;

// 
class QuasiRandomGenerator
{
public:
	QuasiRandomGenerator()
	{
		Init();
	}

	// Returns an uniformely distribued float 
	// value : [0,1)
	float	operator ()() const
	{
		unsigned int im = m_Index++;
		
        // find rightmost zero bit
		int j;
        for (j = 0; j < s_MaxBit; j++, im >>= 1) {
			if ( (im & 1L) == 0 ) break;
        }
			
		m_IX ^= s_IV[j + 0];		// integer values
		return (float) ( s_Factor * m_IX );
			
	}

	// returns a random number distribued 
	// "gaussianly" with a mean and a deviation
	// value : [iMean-iDeviation,iMean+iDeviation)
	float	Gaussian(float iMean, float iDeviation) const
	{
		return GaussianDistribution(*this,iMean,iDeviation);
	}

private:
// Methods
	void					Init()
	{
		#define INDEX(k,j)	[(k) + (j-1)]

		int j, k, l, ipp, niv;
		unsigned int i, mval;
		
		s_IV = new unsigned int[niv = 1 * s_MaxBit];
		
		for (k = 0; k < niv; k++)
			s_IV[k] = 0;
		
		for (k = 0; k < 1; k++)
			s_IV[k] = 1;
		
		mval = 4;
		ipp = 1;
		
		for (k = 1, j = 0; k < niv-1; k += 2)
		{
			s_IV[k] = ipp;
			if (++j == 1)
			{
				mval *= 2;
				ipp += 2;
				j = 0;
			}
			
			if ( ipp > (int)mval )
				ipp = 1;
			
			s_IV[k+1] = ipp;
			if (++j == 1)
			{
				mval *= 2;
				ipp += 2;
				j = 0;
			}
			else
			{
				ipp += 2;
				if ( ipp > (int)mval )
					ipp = 1;
			}
			
			
		}
		
		for (k = 0; k < 1; k++)
		{
			// normalize the set s_IV values
			for (j = 1; j <= mdeg[k]; j++)
				s_IV INDEX(k,j) *= (1L << (s_MaxBit - j));
			
			// calcululate the rest of the s_IV values
			for (j = mdeg[k] + 1; j <= s_MaxBit; j++)
			{
				ipp = ip[k];
				
				// calculate Gray code of s_IV
				i = s_IV INDEX(k, j - mdeg[k]);
				i ^= i / (1L << mdeg[k]);
				
				for (l = mdeg[k] - 1; l >= 1; l--)
				{
					if ( ipp & 1 )
						i ^= s_IV INDEX(k, j-l);
					ipp /= 2;
				}
				
				s_IV INDEX(k,j) = i;
				
			}
		}
		
		s_Factor = 1.0 / (1L << s_MaxBit);
	}
// Members
	static const int		s_MaxBit;
	static unsigned int*	s_IV;
	static double			s_Factor;
	static const int		ip[];
	static const int		mdeg[];

	mutable unsigned int	m_Index;
	mutable unsigned int	m_IX;
};
const int		QuasiRandomGenerator::s_MaxBit = 30;
unsigned int*	QuasiRandomGenerator::s_IV;
double			QuasiRandomGenerator::s_Factor;

// the primitive polynomial coefficients for up to degree 8
const int QuasiRandomGenerator::ip[] = { 0, 1, 1, 2, 1, 4, 2, 4, 7, 11, 13, 14,
			  1, 13, 16, 19, 22, 25,
			  1, 4, 7, 8, 14, 19, 21, 28, 31, 32, 37, 41, 42,
			  50, 55, 56, 59, 62,
			  14, 21, 22, 38, 47, 49, 50, 52, 56, 67, 70, 84,
			  97, 103, 115, 122 };
const int QuasiRandomGenerator::mdeg[] = { 1, 2, 3, 3, 4, 4, 5, 5, 5, 5, 5, 5,
			    6, 6, 6, 6, 6, 6,
			    7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			    8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8
			     };

// 
const QuasiRandomGenerator& 
GetQuasiRandomGenerator() 
{
	static QuasiRandomGenerator qrg;
	return qrg;
}


#endif // RANDOMGENERATOR_H