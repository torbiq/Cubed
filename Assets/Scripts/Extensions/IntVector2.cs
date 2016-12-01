using UnityEngine;
using System.Collections;

public struct IntVector2 {

    private int _x,
                _z;

    public IntVector2(int x, int z) {
        _x = x;
        _z = z;
    }

    public int x
    {
        get { return _x; }
    }

    public int z
    {
        get { return _z; }
    }

    public void GrowX() {
        _x++;
    }

    public void GrowZ() {
        _z++;
    }

    public static IntVector2 operator +(IntVector2 left, IntVector2 right) {
        left._x += right._x;
        left._z += right._z;
        return left;
    }

    public static IntVector2 operator -(IntVector2 left, IntVector2 right) {
        left._x -= right._x;
        left._z -= right._z;
        return left;
    }

    public static bool operator ==(IntVector2 left, IntVector2 right) {
        return (left.x == right.x && left.z == right.z);
    }

    public static bool operator !=(IntVector2 left, IntVector2 right) {
        return (left.x != right.x && left.z != right.z);
    }

    public void MoveD() {
        _z += 1;
    }

    public void MoveU() {
        _z -= 1;
    }
    
    public void MoveR() {
        _x += 1;
    }
    
    public void MoveL() {
        _x -= 1;
    }
    
    public void MoveRD() {
        _x += 1;
        _z += z;
    }
    
    public void MoveLD() {
        _x -= 1;
        _z += z;
    }

    public void MoveRU() {
        _x += 1;
        _z -= z;
    }
    
    public void MoveLU() {
        _x -= 1;
        _z -= z;
    }

    public IntVector2 GetD() {
        return new IntVector2(_x, _z + 1);
    }

    public IntVector2 GetU() {
        return new IntVector2(_x, _z - 1);
    }

    public IntVector2 GetR() {
        return new IntVector2(_x + 1, _z);
    }

    public IntVector2 GetL() {
        return new IntVector2(_x - 1, _z);
    }

    public IntVector2 GetRD() {
        return new IntVector2(_x + 1, _z + 1);
    }

    public IntVector2 GetLD() {
        return new IntVector2(_x - 1, _z + 1);
    }

    public IntVector2 GetRU() {
        return new IntVector2(_x + 1, _z - 1);
    }

    public IntVector2 GetLU() {
        return new IntVector2(_x - 1, _z - 1);
    }
}
