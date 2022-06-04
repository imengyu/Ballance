#ifndef VXTIMEPROFILER_H
#define VXTIMEPROFILER_H

#include "XUtil.h"

/*************************************************
{filename:VxTimeProfiler}
Name: VxTimeProfiler
Summary: Class for profiling purposes

Remarks:
    This class provides methods to accurately compute
    the time elapsed.
Example:
      // To profile several items :

      VxTimeProfiler MyProfiler;
      ...
      float delta_time=MyProfiler.Current();
      MyProfiler.Reset();
      ...
      float delta_time2=MyProfiler.Current();
See also:
*************************************************/
class VX_EXPORT VxTimeProfiler
{
public:
    /*************************************************
    Name: VxTimeProfiler
    Summary: Starts profiling
    *************************************************/
    VxTimeProfiler() { Reset(); }

    // operator =
    VxTimeProfiler &operator=(const VxTimeProfiler &t);

    /*************************************************
    Summary: Restarts the timer
    *************************************************/
    void Reset();

    /*************************************************
    Summary: Returns the current time elapsed (in milliseconds)
    *************************************************/
    float Current();

    /*************************************************
    Summary: Returns the current time elapsed (in milliseconds)
    *************************************************/
    float Split()
    {
        float c = Current();
        Reset();
        return c;
    }

protected:
    DWORD Times[4];
};

#endif