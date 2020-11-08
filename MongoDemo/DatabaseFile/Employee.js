/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MongoDB
 Source Server Version : 40401
 Source Host           : 127.0.0.1:27017
 Source Schema         : Employee

 Target Server Type    : MongoDB
 Target Server Version : 40401
 File Encoding         : 65001

 Date: 08/11/2020 13:27:21
*/


// ----------------------------
// Collection structure for EmployeeDetails
// ----------------------------
db.getCollection("EmployeeDetails").drop();
db.createCollection("EmployeeDetails");

// ----------------------------
// Documents of EmployeeDetails
// ----------------------------
session = db.getMongo().startSession();
session.startTransaction();
db = session.getDatabase("Employee");
db.getCollection("EmployeeDetails").insert([ {
    _id: ObjectId("5f91a384885bdb5c90c74449"),
    Name: "Aufa",
    Department: "Human Resource",
    Address: "Bekasi",
    City: "Bekasi",
    Country: "Indonesia"
} ]);
session.commitTransaction(); session.endSession();

// ----------------------------
// Collection structure for Movies
// ----------------------------
db.getCollection("Movies").drop();
db.createCollection("Movies");

// ----------------------------
// Documents of Movies
// ----------------------------
session = db.getMongo().startSession();
session.startTransaction();
db = session.getDatabase("Employee");
db.getCollection("Movies").insert([ {
    _id: ObjectId("5f95d313049a8c8789d0db09"),
    title: "12 Angry Men",
    director: "Sidney Lumet",
    year: 1957,
    budget: 350000
}, {
    _id: ObjectId("5f95d381049a8c8789d0db0a"),
    title: "Shutter Island",
    director: "Martin Scorsese",
    year: 2010,
    budget: 80000000,
    genre: [
        "Mystery",
        "Thriller"
    ]
} ]);
session.commitTransaction(); session.endSession();
