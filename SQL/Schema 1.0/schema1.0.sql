CREATE DATABASE  IF NOT EXISTS `quizzy` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `quizzy`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: quizzy
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `enrollments`
--

DROP TABLE IF EXISTS `enrollments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `enrollments` (
  `subjectID` int DEFAULT NULL,
  `studentID` int DEFAULT NULL,
  `status` tinyint(1) DEFAULT NULL,
  KEY `subjectID` (`subjectID`),
  KEY `studentID` (`studentID`),
  CONSTRAINT `enrollments_ibfk_1` FOREIGN KEY (`subjectID`) REFERENCES `subjects` (`subjectID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `enrollments_ibfk_2` FOREIGN KEY (`studentID`) REFERENCES `students` (`studentId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `login_details`
--

DROP TABLE IF EXISTS `login_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `login_details` (
  `email` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL,
  `role` enum('teacher','student') DEFAULT NULL,
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mcq_answers`
--

DROP TABLE IF EXISTS `mcq_answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mcq_answers` (
  `mcqID` int DEFAULT NULL,
  `studentID` int DEFAULT NULL,
  `answer` enum('A','B','C','D') DEFAULT NULL,
  KEY `mcqID` (`mcqID`),
  KEY `studentID` (`studentID`),
  CONSTRAINT `mcq_answers_ibfk_1` FOREIGN KEY (`mcqID`) REFERENCES `mcqs` (`mcqID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `mcq_answers_ibfk_2` FOREIGN KEY (`studentID`) REFERENCES `students` (`studentId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mcq_lookup`
--

DROP TABLE IF EXISTS `mcq_lookup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mcq_lookup` (
  `mcqID` int DEFAULT NULL,
  `quizID` int DEFAULT NULL,
  KEY `mcqID` (`mcqID`),
  KEY `quizID` (`quizID`),
  CONSTRAINT `mcq_lookup_ibfk_1` FOREIGN KEY (`mcqID`) REFERENCES `mcqs` (`mcqID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `mcq_lookup_ibfk_2` FOREIGN KEY (`quizID`) REFERENCES `quiz` (`quizID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mcqs`
--

DROP TABLE IF EXISTS `mcqs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mcqs` (
  `mcqID` int NOT NULL AUTO_INCREMENT,
  `option_A` varchar(50) DEFAULT NULL,
  `option_B` varchar(50) DEFAULT NULL,
  `option_C` varchar(50) DEFAULT NULL,
  `option_D` varchar(50) DEFAULT NULL,
  `correct_opt` enum('A','B','C','D') DEFAULT NULL,
  `image_path` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`mcqID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `quiz`
--

DROP TABLE IF EXISTS `quiz`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quiz` (
  `quizID` int NOT NULL AUTO_INCREMENT,
  `isPublic` tinyint(1) DEFAULT NULL,
  `quiz_name` varchar(100) DEFAULT NULL,
  `given_time` int DEFAULT NULL,
  PRIMARY KEY (`quizID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `quiz_lookup`
--

DROP TABLE IF EXISTS `quiz_lookup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quiz_lookup` (
  `quizID` int DEFAULT NULL,
  `subjectID` int DEFAULT NULL,
  KEY `quizID` (`quizID`),
  KEY `subjectID` (`subjectID`),
  CONSTRAINT `quiz_lookup_ibfk_1` FOREIGN KEY (`quizID`) REFERENCES `quiz` (`quizID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quiz_lookup_ibfk_2` FOREIGN KEY (`subjectID`) REFERENCES `subjects` (`subjectID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `result_lookup`
--

DROP TABLE IF EXISTS `result_lookup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `result_lookup` (
  `resultID` int DEFAULT NULL,
  `quizID` int DEFAULT NULL,
  `studentID` int DEFAULT NULL,
  KEY `resultID` (`resultID`),
  KEY `quizID` (`quizID`),
  KEY `studentID` (`studentID`),
  CONSTRAINT `result_lookup_ibfk_1` FOREIGN KEY (`resultID`) REFERENCES `results` (`resultID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `result_lookup_ibfk_2` FOREIGN KEY (`quizID`) REFERENCES `quiz` (`quizID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `result_lookup_ibfk_3` FOREIGN KEY (`studentID`) REFERENCES `students` (`studentId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `results`
--

DROP TABLE IF EXISTS `results`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `results` (
  `resultID` int NOT NULL AUTO_INCREMENT,
  `mcq_marks` int DEFAULT NULL,
  `shq_marks` int DEFAULT NULL,
  `total_marks` int DEFAULT NULL,
  PRIMARY KEY (`resultID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `short_questions`
--

DROP TABLE IF EXISTS `short_questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `short_questions` (
  `shqID` int NOT NULL AUTO_INCREMENT,
  `question` text,
  `image_path` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`shqID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shq_answers`
--

DROP TABLE IF EXISTS `shq_answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shq_answers` (
  `shqID` int DEFAULT NULL,
  `studentID` int DEFAULT NULL,
  `answer` text,
  KEY `shqID` (`shqID`),
  KEY `studentID` (`studentID`),
  CONSTRAINT `shq_answers_ibfk_1` FOREIGN KEY (`shqID`) REFERENCES `short_questions` (`shqID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shq_answers_ibfk_2` FOREIGN KEY (`studentID`) REFERENCES `students` (`studentId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shq_check`
--

DROP TABLE IF EXISTS `shq_check`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shq_check` (
  `shqID` int DEFAULT NULL,
  `studentID` int DEFAULT NULL,
  `marks` enum('0','0.5','1','1.5','2') DEFAULT NULL,
  KEY `shqID` (`shqID`),
  KEY `studentID` (`studentID`),
  CONSTRAINT `shq_check_ibfk_1` FOREIGN KEY (`shqID`) REFERENCES `short_questions` (`shqID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shq_check_ibfk_2` FOREIGN KEY (`studentID`) REFERENCES `students` (`studentId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shq_lookup`
--

DROP TABLE IF EXISTS `shq_lookup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shq_lookup` (
  `shqID` int DEFAULT NULL,
  `quizID` int DEFAULT NULL,
  KEY `shqID` (`shqID`),
  KEY `quizID` (`quizID`),
  CONSTRAINT `shq_lookup_ibfk_1` FOREIGN KEY (`shqID`) REFERENCES `short_questions` (`shqID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shq_lookup_ibfk_2` FOREIGN KEY (`quizID`) REFERENCES `quiz` (`quizID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `studentId` int NOT NULL,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `roll_num` int DEFAULT NULL,
  `dept` char(2) DEFAULT NULL,
  `addmission_year` year DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`studentId`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `subjects`
--

DROP TABLE IF EXISTS `subjects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subjects` (
  `subjectID` int NOT NULL,
  `subject_name` varchar(50) DEFAULT NULL,
  `subject_code` char(5) DEFAULT NULL,
  `teacherId` int DEFAULT NULL,
  PRIMARY KEY (`subjectID`),
  KEY `teacherId` (`teacherId`),
  CONSTRAINT `subjects_ibfk_1` FOREIGN KEY (`teacherId`) REFERENCES `teachers` (`teacherId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `teachers`
--

DROP TABLE IF EXISTS `teachers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `teachers` (
  `teacherId` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`teacherId`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-22  0:41:03
